using exp.Backend.Auth;
using exp.Backend.Helpers;
using exp.Models.Helpers;
using exp.Models.ViewModels;
using exp.Services.Email;
using exp.Services.Organisations;
using exp.Services.OrganisationSurveys;
using Ganss.Xss;
using IdentityPasswordGenerator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static exp.Backend.Auth.AccountModel;

namespace exp.Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IBasicRegisterMethods _basicRegisterMethods;
        private readonly VaultSettings _vaultSettings;
        public HtmlSanitizer sanitizer = new HtmlSanitizer();
        private readonly IOrganisationService _organisationService;
        private readonly IOrganisationSurveyService _organisationSurveyService;
        private readonly IRecaptchaService _recaptchaService;

        public AccountController(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IBasicRegisterMethods basicRegisterMethods,
            VaultSettings vaultSettings,
            IOrganisationSurveyService organisationSurveyService,
            IRecaptchaService recaptchaService,
            IOrganisationService organisationService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _basicRegisterMethods = basicRegisterMethods;
            _configuration = configuration;
            _vaultSettings = vaultSettings;
            _organisationSurveyService = organisationSurveyService;
            _recaptchaService = recaptchaService;
            _organisationService = organisationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null && userExists.IsDeleted != true)
            {
                return StatusCode(StatusCodes.Status409Conflict, "User already exists!");
            }

            var sanitizeModel = new RegisterModel
            {
                Email = sanitizer.Sanitize(model.Email),
                Roles = model.Roles != null ? model.Roles.Select(x => sanitizer.Sanitize(x)).ToList() : new List<string>(),
                Name = model.Name
            };


            var passwordGenerator = new PasswordGenerator();
            var options = _userManager.Options.Password;
            var password = passwordGenerator.GeneratePassword(options);

            if (userExists != null && userExists.IsDeleted == true)
            {
                userExists.IsDeleted = false;
                string passwordHash = _userManager.PasswordHasher.HashPassword(userExists, password);
                userExists.PasswordHash = passwordHash;
                var result = await _userManager.UpdateAsync(userExists);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");
                }
                foreach (var role in sanitizeModel.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        await _userManager.AddToRoleAsync(userExists, role);
                    }
                }
            }
            else
            {
                var user = new ApplicationUser
                {
                    Email = sanitizeModel.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = sanitizeModel.Email,
                    EmailConfirmed = true,
                    TwoFactorEnabled = true,
                    Name = sanitizeModel.Name,
                };
                string passwordHash = _userManager.PasswordHasher.HashPassword(user, password);
                user.PasswordHash = passwordHash;
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");
                }
                foreach (var role in sanitizeModel.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            string html = $"Your password: " + password + "<br>For access in app follow link " + _vaultSettings.ApplicationUrl+ "/login";
            _emailService.Send(sanitizeModel.Email, "Occtet app account", html);


            return Ok("User created successfully!");
        }


        [HttpGet]
        [Authorize(Roles = ("Admin"))]
        [Route("createRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };
            await _roleManager.CreateAsync(role);
            return Ok();
        }
        #region Register Organisation 
        [AllowAnonymous]
        [HttpPost("registerOrganisation")]
        public async Task<IActionResult> RegisterOrganisation(RegisterRequestOrganisationViewModel organisation)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(organisation.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            if (organisation.Agreement == false)
            {
                throw new Exception();
            }
            var password = GenerateAccessCode.GeneratePassword();
            var user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                Name = organisation.Name,
            };

            string passwordHash = _userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");
            }

            if (await _roleManager.RoleExistsAsync("Guest"))
            {
                await _userManager.AddToRoleAsync(user, "Guest");
            }
            try
            {
                var newOrganisation = await _organisationService.RegisterOrganisation(organisation, password, user.Id);

                if (organisation?.OrganisationSurveyId != null)
                {
                    await _organisationSurveyService.AddOrganizationAtSurvey(newOrganisation.Item1, (int)organisation.OrganisationSurveyId);
                }
                return Ok(newOrganisation.Item2);
            }
            catch (Exception error)
            {
                if (error.Message != "Organisation already exists!")
                {
                    await _userManager.DeleteAsync(user);
                    throw error ?? new Exception();
                }
                else
                {
                    throw error ?? new Exception();
                }
            }
            return BadRequest();
        }
        #endregion
    }
}
