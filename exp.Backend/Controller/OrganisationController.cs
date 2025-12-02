using exp.backend.Auth;
using exp.Backend.Helpers;
using exp.Models.ViewModels;
using exp.Services.Organisations;
using exp.Services.OrganisationSurveys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exp.Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Guest")]
    public class OrganisationController : ControllerBase
    {
        private readonly IOrganisationService _organisationService;
        private readonly IOrganisationSurveyService _organisationSurveyService;
        private readonly IRecaptchaService _recaptchaService;

        public OrganisationController(IOrganisationService organizationService, IOrganisationSurveyService organisationSurveyService, IRecaptchaService recaptchaService)
        {
            _organisationService = organizationService;
            _organisationSurveyService = organisationSurveyService;
            _recaptchaService = recaptchaService;
        }
        [AllowAnonymous]
        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            var result = await _organisationService.ConfirmEmail(code);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("resendCode")]
        public IActionResult ResendCode(string code)
        {
            _organisationService.ResendCode(code);

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("sendEmailToOwner")]
        public async Task<IActionResult> SendEmailToOwner([Sanitize] string organisationVat, [Sanitize] string memberRequestEmail, [Sanitize] string memberRequestName, string captchaToken)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(captchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            _organisationService.SendEmailToOwner(organisationVat, memberRequestEmail, memberRequestName);

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("acceptMemberRequest")]
        public async Task<IActionResult> AcceptMemberRequest([Sanitize] string memberRequestEmail, string organisationVat, string accessCode, string captchaToken)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(captchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            _organisationService.AcceptMemberRequest(memberRequestEmail, organisationVat, accessCode);

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs(ContactUsViewModel message)
        {
            var isHuman = await _recaptchaService.VerifyTokenAsync(message.CaptchaToken);

            if (!isHuman)
            {
                return BadRequest(new { success = false, message = "Bot detected, access denied" });
            }
            _organisationService.ContactUs(message);

            return Ok();
        }
        [HttpGet("getOrganisation")]
        public IActionResult GetOrganisation()
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = _organisationService.GetOrgansation(organisationId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("getActivitySectorsDetails")]
        public IActionResult GetActivitySectorsDetails(string? searchText = null)
        {
            var result = _organisationService.GetActivitySectorsDetails(searchText);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getActivitySectors")]
        public IActionResult GetActivitySectors()
        {
            var result = _organisationService.GetActivitySectors();

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getDeliveryCountries")]
        public async Task<IActionResult> GetDeliveryCountries(string? searchText = null)
        {
            var result = await _organisationService.GetDeliveryCountries(searchText);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var result = await _organisationService.GetCountries();

            return Ok(result);
        }
        [HttpPut("updateOrganisation")]
        public async Task<IActionResult> Update(UpdateOrganisationViewModel organization)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());

            await _organisationService.Update(organization, organisationId);

            return Ok();
        }
    }
}
