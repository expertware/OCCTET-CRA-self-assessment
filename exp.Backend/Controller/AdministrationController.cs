using exp.backend.Auth;
using exp.Models.ViewModels;
using exp.Services.OrganisationAdministrationService;
using exp.Services.OrganisationSurveys;
using exp.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exp.Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdministrationController : ControllerBase
    {
        private readonly IOrganisationAdministrationService _organisationAdministrationService;
        private readonly IUserService _userService;
        private readonly IOrganisationSurveyService _organisationSurveyService;
        public AdministrationController(IOrganisationAdministrationService organisationAdministrationService, IUserService userService, IOrganisationSurveyService organisationSurveyService)
        {
            _organisationAdministrationService = organisationAdministrationService;
            _userService = userService;
            _organisationSurveyService = organisationSurveyService;
        }

        #region Organisations
        [Authorize(Roles = ("Admin"))]
        [HttpPost("resendCode")]
        public async Task<IActionResult> ResendCode(int organisationId)
        {
            await _organisationAdministrationService.ResendCodeAdm(organisationId);

            return Ok();
        }

        [Authorize(Roles = ("Admin"))]
        [HttpGet("getOrgansation")]
        public IActionResult GetOrgansation(int organisationId)
        {
            var result = _organisationAdministrationService.GetOrgansation(organisationId);

            return Ok(result);
        }
        [Authorize(Roles = "Admin, Request Approver")]
        [HttpGet("getRequestOrgansation")]
        public IActionResult GetRequestOrgansation(int organisationId)
        {
            var result = _organisationAdministrationService.GetRequestOrgansation(organisationId);

            return Ok(result);
        }
        [Authorize(Roles = "Admin, Request Approver")]

        [HttpGet("getHistorySurvey")]
        public async Task<IActionResult> GetHistorySurvey(int organisationId, int surveyId)
        {
            var result = await _organisationSurveyService.GetHistorySurveyAdm(organisationId, surveyId);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getScoreSurveyReports")]
        public IActionResult GetScoreSurveyReports(int organisationSurveyId)
        {
            var result = _organisationSurveyService.GetScoreSurveyReports(organisationSurveyId);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getOrganizationSurvey")]
        public async Task<IActionResult> GetOrganizationSurvey(int organisationSurveyId)
        {
            var result = await _organisationSurveyService.GetOrganizationSurvey(organisationSurveyId, true);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getSectionProgress")]
        public async Task<IActionResult> GetSectionProgress(int organizationSurveyId, int organisationId)
        {
            var result = await _organisationSurveyService.GetSectionProgress(organizationSurveyId, organisationId);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("getOrganizationSurveys")]
        public IActionResult GetOrganizationSurveys(FilterPagingSurveyAdmViewModel filter)
        {
            var result = _organisationSurveyService.GetOrganizationSurveysForAdm(filter);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("getOrganisations")]
        public IActionResult GetOrganisations(FilterCompanies filterCompanies)
        {
            var result = _organisationAdministrationService.GetOrganisations(filterCompanies);

            return Ok(result);
        }
        [Authorize(Roles = "Admin, Request Approver")]
        [HttpPost("getRequestsOrganisations")]
        public IActionResult GetRequestsOrganisations(FilterCompanies filterCompanies)
        {
            var result = _organisationAdministrationService.GetRequestsOrganisations(filterCompanies);

            return Ok(result);
        }
        [Authorize(Roles = "Admin, Request Approver")]
        [HttpPost("respondRequest")]
        public async Task<IActionResult> RespondRequest(RespondeRequestViewModel response)
        {
            await _organisationAdministrationService.RespondRequest(response);

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int organisationId)
        {
            await _organisationAdministrationService.Delete(organisationId);

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updateOrganisation")]
        public async Task<IActionResult> Update(UpdateOrganisationAdmViewModel organisation)
        {
            await _organisationAdministrationService.Update(organisation);

            return Ok();
        }
        #endregion

        #region Users
        [Authorize(Roles = "Admin")]
        [HttpGet("getUserForEdit")]
        public IActionResult GetUserForEdit(string userId)
        {
            var result = _userService.GetUserForEdit(userId);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getRoles")]
        public IActionResult GetRoles()
        {
            var result = _userService.GetRoles();

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("getUsers")]
        public IActionResult GetUsers(FilterUsers filter)
        {
            var result = _userService.GetUsers(filter);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updateUser")]
        public async Task<IActionResult> Update(UpdateUserDetailsViewModel user)
        {
            await _userService.Update(user);

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteUser")]
        public async Task<IActionResult> Delete(string userId)
        {
            await _userService.Delete(userId);

            return Ok();
        }
        #endregion
    }
}
