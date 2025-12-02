using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace exp.Backend.Auth
{
    public class AccountModel
    {
        public class RegisterModel
        {
            [EmailAddress]
            [Required(ErrorMessage = "Email is required")]
            [Display(Name = "Email")]
            public string Email { get; set; } = default!;
            [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            public string Name { get; set; } = default!;
            public List<string>? Roles { get; set; }
        }
        public class LoginModel
        {
            [Required(ErrorMessage = "User Name is required")]
            public string Username { get; set; } = default!;

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; } = default!;
            [Required]
            public string CaptchaToken { get; set; } = default!;

        }
        public class ResetPasswordModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = default!;

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 12)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = default!;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = default!;
            [Required]
            public string Code { get; set; } = default!;
            [Required]
            public string CaptchaToken { get; set; } = default!;
        }
        public class ResetAccessCodeModel
        {
            [Required]
            public string Code { get; set; } = default!;
            [Required]
            public string Vat { get; set; } = default!;
            [Required]
            public string CaptchaToken { get; set; } = default!;
        }
        public class ForgotPasswordModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = default!;
            [Required]
            public string CaptchaToken { get; set; } = default!;
        }
        public class ForgotAccessCodeModel
        {
            [Required]
            [Display(Name = "Vat")]
            public string Vat { get; set; } = default!;
            [Required]
            public string CaptchaToken { get; set; } = default!;
        }
        public class FacebookModel
        {
            public string Email { get; set; } = null!;
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
        }
        public class UserLoginModel
        {
            public string Email { get; set; } = null!;
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
        }
        public class TwoFactorDto
        {
            [Required]
            public string Email { get; set; } = default!;
            [Required]
            public string Provider { get; set; } = default!;
            [Required]
            public string Token { get; set; } = default!;
        }
        public class AuthResponse
        {
            public bool Is2FactorRequired { get; set; }
            public string Provider { get; set; } = default!;
        }
        public class TokenModel
        {
            [Required]
            public string AccessToken { get; set; } = default!;
            [Required]
            public string RefreshToken { get; set; } = default!;
        }
        public class SignInOrganisation
        {
            [Required]
            public string AccessCode { get; set; } = default!;
            [Required]
            public string CaptchaToken { get; set; } = default!;
            public int? OrganisationSurveyId { get; set; } = default!;
        }
    }
}
