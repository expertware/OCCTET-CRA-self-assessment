using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using exp.backend.Auth;
using exp.Backend.Auth;
using exp.Backend.Data;
using exp.Backend.Helpers;
using exp.Infrastructure.Context;
using exp.Infrastructure.Repositories.ActivitySectors;
using exp.Infrastructure.Repositories.Attachments;
using exp.Infrastructure.Repositories.Countries;
using exp.Infrastructure.Repositories.Organisations;
using exp.Infrastructure.Repositories.OrganisationSurveys;
using exp.Infrastructure.Repositories.Questions;
using exp.Infrastructure.Repositories.Roles;
using exp.Infrastructure.Repositories.Sections;
using exp.Infrastructure.Repositories.SurveyResponses;
using exp.Infrastructure.Repositories.Surveys;
using exp.Infrastructure.Repositories.Users;
using exp.Infrastructure.Repositories.VwOrganisationResults;
using exp.Models.Helpers;
using exp.Services.CopilotAgent;
using exp.Services.Email;
using exp.Services.OrganisationAdministrationService;
using exp.Services.Organisations;
using exp.Services.OrganisationSurveys;
using exp.Services.Questions;
using exp.Services.Reports;
using exp.Services.Surveys;
using exp.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;


ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);
#region Key Volt

var keyVaultUri = builder.Configuration.GetSection("KeyVault:KeyVaultURL").Value;
var clientId = builder.Configuration.GetSection("KeyVault:ClientId").Value;
var clientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret").Value;
var directoryId = builder.Configuration.GetSection("KeyVault:DirectoryId").Value;

var credential = new ClientSecretCredential(directoryId!.ToString(), clientId!.ToString(), clientSecret!.ToString());
var secretClient = new SecretClient(new Uri(keyVaultUri!.ToString()), credential);

#endregion
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//Add services to the container.
var connectionString = secretClient.GetSecret("ConnectionString");
// Initialiaze with your DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString.Value.Value));

builder.Services.AddDbContext<SurveyDBContext>(options =>
    options.UseNpgsql(connectionString.Value.Value));


VaultSettings vaultSettings = new VaultSettings()
{
    #region JWTToken
    JWTTokenSecret = secretClient.GetSecret("JWTTokenSecret").Value.Value,
    #endregion
    #region SMTP
    SmtpHost = secretClient.GetSecret("SmtpHost").Value.Value,
    SmtpPort = secretClient.GetSecret("SmtpPort").Value.Value,
    EmailFrom = secretClient.GetSecret("EmailFrom").Value.Value,
    SmtpUser = secretClient.GetSecret("SmtpUser").Value.Value,
    SmtpPass = secretClient.GetSecret("SmtpPass").Value.Value,
    #endregion

    ApplicationUrl = secretClient.GetSecret("ApplicationUrl").Value.Value,
    EmailContact = secretClient.GetSecret("EmailContact").Value.Value,
    RecaptchaSecretKey = secretClient.GetSecret("RecaptchaSecretKey").Value.Value,
    AgentCopilotBearer = secretClient.GetSecret("AgentCopilotBearer").Value.Value

};

#region Identity Conf
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 5;
});
#endregion
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(vaultSettings.JWTTokenSecret))
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            string username = context.Principal!.FindFirstValue(ClaimTypes.Name)!;
            var tokenStamp = context.Principal!.FindFirstValue("security_stamp");

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                context.Fail("User not found");
                return;
            }

            var currentStamp = await userManager.GetSecurityStampAsync(user);
            if (currentStamp != tokenStamp)
            {
                context.Fail("Token invalid");
            }
        }
    };
});
// Configure expired refresh token time
_ = double.TryParse(builder.Configuration.GetSection("JWT:RefreshTokenValidityInDays").Value, out double refreshTokenValidityInDays);
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromDays(refreshTokenValidityInDays);
    options.Name = "Default";
});

// caching
builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    var cacheProfiles = builder.Configuration
            .GetSection("CacheProfiles")
            .GetChildren();

});

builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSingleton(vaultSettings);

// dependence injection
builder.Services.AddHttpClient<IRecaptchaService, RecaptchaService>();

builder.Services.AddScoped<IBasicRegisterMethods, BasicRegisterMethods>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IOrganisationService, OrganisationService>();

builder.Services.AddScoped<IOrganizationSurveyRepository, OrganizationSurveyRepository>();
builder.Services.AddScoped<IOrganisationSurveyService, OrganisationSurveyService>();

builder.Services.AddScoped<ISurveyResponseRepository, SurveyResponseRepository>();

builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<ISectionSurveyRepository, SectionSurveyRepository>();
builder.Services.AddScoped<ISurveyContentService, SurveyContentService>();

builder.Services.AddScoped<IQuestionAttachments, QuestionAttachments>();

builder.Services.AddScoped<IOrganizationSurveyRepository, OrganizationSurveyRepository>();

builder.Services.AddScoped<IActivitySectorRepository, ActivitySectorRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddScoped<IOrganisationAdministrationService, OrganisationAdministrationService>();

builder.Services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IVwOrganisationResultRepository, VwOrganisationResultRepository>();

builder.Services.AddScoped<IVwSectionReports, VwSectionReports>();
builder.Services.AddScoped<IOrganisationReportsRepository, OrganisationReportsRepository>();

builder.Services.AddScoped<ICopilotAgentService, CopilotAgentService>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"..\exp.Backend\DataProtectionKeys"))
    .SetApplicationName("Surveys");

var myOptions = new MyRateLimitOptions();
builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);
var slidingPolicy = "sliding";

builder.Services.AddRateLimiter(option =>
{
    option.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    option.AddPolicy(slidingPolicy, httpContext =>
    RateLimitPartition.GetSlidingWindowLimiter(partitionKey: httpContext.User?.Identity?.Name
    , factory: _ => new SlidingWindowRateLimiterOptions
    {
        PermitLimit = myOptions.PermitLimit,
        Window = TimeSpan.FromSeconds(myOptions.Window),
        SegmentsPerWindow = myOptions.SegmentsPerWindow,
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        //QueueLimit = myOptions.QueueLimit
    }));
});
Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();

var app = builder.Build();
app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UsePathBase("/api");
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("Content-Security-Policy");
    context.Response.Headers.Append("Content-Security-Policy", "frame-ancestors 'none';");
    await next();
});

app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();


app.Run();
