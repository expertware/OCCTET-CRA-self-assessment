using exp.Backend.Auth;
using exp.Backend.Helpers;
using exp.Models.Helpers;
using exp.Services.Email;
using exp.Services.Organisations;
using exp.Services.OrganisationSurveys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static exp.Backend.Auth.AccountModel;

namespace exp.Backend.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly VaultSettings _vaultSettings;
        private readonly IRecaptchaService _recaptchaService;
        private readonly IOrganisationService _organisationService;
        private readonly IDataProtector _dataProtector;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrganisationSurveyService _organisationSurveyService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmailService emailService,
            VaultSettings vaultSettings,
            IRecaptchaService recaptchaService,
            IOrganisationService organisationService,
             RoleManager<IdentityRole> roleManager,
            IDataProtectionProvider dataProtectionProvider,
            IOrganisationSurveyService organisationSurveyService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
            _vaultSettings = vaultSettings;
            _recaptchaService = recaptchaService;
            _organisationService = organisationService;
            _roleManager = roleManager;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectionKey");
            _organisationSurveyService = organisationSurveyService;
        }


        #region Simple login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(model.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || user.IsDeleted == true)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! User don't found!");
            }

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailConfirmed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! In order to log in, please confirm the email.");
            }

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (checkPassword.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked due to too many failed login attempts.");
            }
            if (!checkPassword.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! Please enter a valid login name and password.");
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return await GenerateOTPFor2Factor(user);
            }
            if (user != null && checkPassword.Succeeded)
            {
                return await LoginAfterValidation(user);
            }

            return Unauthorized();
        }
        private async Task<IActionResult> GenerateOTPFor2Factor(ApplicationUser user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (!providers.Contains("Email"))
            {
                return Unauthorized("Invalid 2-Factor Provider.");
            }
            await _userManager.UpdateSecurityStampAsync(user);
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            string html = $"Your verification code: " + token;
            _emailService.Send(user.Email!, "Authentification token", html);

            return Ok(new AuthResponse { Is2FactorRequired = true, Provider = "Email" });
        }
        [HttpPost("validateOTPFor2Factor")]
        public async Task<IActionResult> ValidateOTPFor2Factor(TwoFactorDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user.IsDeleted == true)
            {
                return BadRequest("Invalid Request");
            }
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, model.Provider, model.Token!);

            if (!isValid)
            {
                return Unauthorized("Invalid OTP token.");
            }

            return await LoginAfterValidation(user);
        }
        #endregion

        #region Common
        private async Task<IActionResult> LoginAfterValidation(ApplicationUser user)
        {
            var securityStamp = await _userManager.GetSecurityStampAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("security_stamp", securityStamp),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", newRefreshToken);

            var token = CreateToken(authClaims);

            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = newRefreshToken
            });
        }
        private async Task<List<Claim>> GetUserClaims(ApplicationUser user, string? refreshId)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var organisationId = _organisationService.ExistAccountOrganisation(user.Id).Id;
            var securityStamp = await _userManager.GetSecurityStampAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName  ?? string.Empty),
                new(ClaimTypes.NameIdentifier, userRoles.Any(x => x == "Guest") ? organisationId.ToString() : user.Id  ),
                new(ClaimTypes.Email, user.Email  ?? string.Empty),
                new Claim("security_stamp", user.SecurityStamp),
                new Claim("refreshId", refreshId),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        authClaims.Add(roleClaim);
                    }
                }
            }
            return await Task.FromResult(authClaims);
        }


        [HttpGet("user-details")]
        [Authorize]
        public IActionResult GetUserDetailsByAccessToken()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var key = Encoding.ASCII.GetBytes(_vaultSettings.JWTTokenSecret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(accessToken, validations, out var tokenSecure);

            return new ObjectResult(new
            {
                UserName = claims.FindAll(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() != "Guest" ? claims.FindFirstValue(ClaimTypes.Name) : null,
                UserId = claims.FindAll(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() != "Guest" ? claims.FindFirstValue(ClaimTypes.NameIdentifier) : null,
                UserRoles = claims.FindAll(x => x.Type == ClaimTypes.Role).Select(x => x.Value),
                AccessCode = claims.FindAll(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() == "Guest" ? claims.FindFirstValue(ClaimTypes.NameIdentifier) : null,
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if (principal?.Identity?.Name == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.IsDeleted == true)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.FirstOrDefault() == "Guest")
            {
                //var deviceId = Request.Headers.UserAgent + Request.Headers.Origin + DateTime.Now;
                var handler = new JwtSecurityTokenHandler();
                var oldToken = handler.ReadJwtToken(tokenModel.AccessToken);
                var refreshId = oldToken.Claims.Select(claim => (claim.Type, claim.Value)).ToList().FirstOrDefault(z => z.Type == "refreshId").Value;

                var currentRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", $"RefreshToken:{refreshId}");
                var isTokenSame = tokenModel.RefreshToken == currentRefreshToken;
                var isTokenValid = await _userManager.VerifyUserTokenAsync(user, "Default", $"RefreshToken:{refreshId}", tokenModel.RefreshToken);
                if (isTokenValid && isTokenSame)
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, "Default", $"RefreshToken:{refreshId}");
                    var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", $"RefreshToken:{refreshId}");
                    await _userManager.SetAuthenticationTokenAsync(user, "Default", $"RefreshToken:{refreshId}", newRefreshToken);
              
                    var authClaims = await GetUserClaims(user, refreshId);
                    var token = CreateToken(authClaims);
                    await _userManager.UpdateAsync(user);
                    return Ok(new
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = newRefreshToken
                    });
                }
            }
            else
            {
                var currentRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");
                var isTokenSame = tokenModel.RefreshToken == currentRefreshToken;
                var isTokenValid = await _userManager.VerifyUserTokenAsync(user, "Default", "RefreshToken", tokenModel.RefreshToken);

                if (isTokenSame && isTokenValid)
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
                    var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");
                    await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", newRefreshToken);

                    var authClaims = await GetUserClaims(user, null);
                    var token = CreateToken(authClaims);
                    await _userManager.UpdateAsync(user);
                    return Ok(new
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = newRefreshToken
                    });
                }
            }
            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (this.User.Identity is null)
            {
                return BadRequest("Invalid user name");
            }
            else
            {
                string userName = User.Identity.Name ?? string.Empty;

                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return BadRequest("Invalid user name");
                }
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
                await _userManager.UpdateSecurityStampAsync(user);
            }
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(model.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByNameAsync(model.Email);
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user) || user.IsDeleted == true)
                {
                    return BadRequest("Invalid user!");
                }

                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var appUrl = _vaultSettings.ApplicationUrl;
                var callbackUrl = $"{appUrl}/Reset-password?email={user.Email}&code={System.Web.HttpUtility.UrlEncode(code)}";
                _emailService.Send(user.Email!, "Occtet - Reset pasword", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //return RedirectToAction("ForgotPasswordConfirmation", "Account");
                return Ok();
            }

            return BadRequest("Something wrong!");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(model.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Something wrong!");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || user.IsDeleted == true)
            {
                return BadRequest("Invalid user!");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Something wrong!");
            }

            return Ok("Password reset successfully!");
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_vaultSettings.JWTTokenSecret));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_vaultSettings.JWTTokenSecret)),
                ValidateLifetime = false,
                //ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        #endregion

        #region Organisation
        [HttpPost]
        [Route("signInOrganisation")]
        public async Task<IActionResult> SignInOrganisation(SignInOrganisation model)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(model.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            model.AccessCode = model.AccessCode.Trim();
            var organization = _organisationService.ExistOrganisation(model.AccessCode);
            var user = await _userManager.FindByIdAsync(organization.AccountId);

            if (user == null || user.IsDeleted == true)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! User don't found!");
            }

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, model.AccessCode, lockoutOnFailure: true);
            if (checkPassword.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked due to too many failed login attempts.");
            }
            if (!checkPassword.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! Please enter a valid login name and password.");
            }

            if (user != null && checkPassword.Succeeded)
            {
                if (model.OrganisationSurveyId != null)
                {
                    await _organisationSurveyService.AddOrganizationAtSurvey(organization.Id, (int)model.OrganisationSurveyId);
                }
                var securityStamp = await _userManager.GetSecurityStampAsync(user);
                var refreshId = Guid.NewGuid().ToString();
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, organization.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim("security_stamp", securityStamp),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("refreshId", refreshId),
                };
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                //var deviceId = Request.Headers.UserAgent + Request.Headers.Origin + DateTime.Now;
                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", $"RefreshToken:{refreshId}");
                var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", $"RefreshToken:{refreshId}");
                await _userManager.SetAuthenticationTokenAsync(user, "Default", $"RefreshToken:{refreshId}", newRefreshToken);


                var token = CreateToken(authClaims);
                await _userManager.UpdateAsync(user);
                return Ok(new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = newRefreshToken
                });
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-accessCode")]
        public async Task<ActionResult> ForgotAccessCode(ForgotAccessCodeModel model)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(model.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            if (ModelState.IsValid)
            {
                var organization = _organisationService.ExistOrganisationByVat(model.Vat);
                var user = await _userManager.FindByIdAsync(organization.AccountId);

                if (user == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! User don't found!");
                }
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return BadRequest("Invalid user!");
                }

                var appUrl = _vaultSettings.ApplicationUrl;
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = $"{appUrl}/Reset-access-code?vat={organization.Vat}&code={System.Web.HttpUtility.UrlEncode(code)}";
                _emailService.Send(_dataProtector.Unprotect(organization.ContactPersonEmail!), "Occtet - Reset access code", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return Ok();
            }

            return BadRequest("Something wrong!");
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("reset-access-code")]
        public async Task<ActionResult> ResetAccessCode(ResetAccessCodeModel model)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(model.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Something wrong!");
            }

            var organization = _organisationService.ExistOrganisationByVat(model.Vat);
            var user = await _userManager.FindByIdAsync(organization.AccountId);
            if (user == null)
            {
                return BadRequest("Invalid user!");
            }
            var password = GenerateAccessCode.GeneratePassword();

            var oldAccessCode = await _organisationService.ResetAccessCode(model.Vat, password);

            var result = await _userManager.ResetPasswordAsync(user, System.Web.HttpUtility.UrlDecode(model.Code), password);

            if (!result.Succeeded)
            {
                await _organisationService.ResetAccessCode(model.Vat, oldAccessCode);
                return BadRequest("Something wrong!");
            }
            var appUrl = _vaultSettings.ApplicationUrl;
            string organizationName = System.Net.WebUtility.HtmlEncode(organization.Name);

            var bodyText = $"Here is your access code for {organizationName}:";

            _emailService.SendEmailAccessCode(_dataProtector.Unprotect(organization.ContactPersonEmail), appUrl!, _dataProtector.Unprotect(organization.AccessCode), bodyText, organization.Vat, organization.Name);

            return Ok("Password reset successfully!");
        }
        #endregion
    }
}
