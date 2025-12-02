using exp.backend.Auth;
using exp.Models.ViewModels;
using exp.Services.OrganisationSurveys;
using exp.Services.Surveys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exp.Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Guest")]
    public class SurveyController : ControllerBase
    {
        private readonly IOrganisationSurveyService _organisationSurveyService;
        private readonly ISurveyService _surveyService;

        public SurveyController(IOrganisationSurveyService organisationSurveyService, ISurveyService surveyService)
        {
            _organisationSurveyService = organisationSurveyService;
            _surveyService = surveyService;
        }
        [HttpPost("startSurvey/{surveyId}")]
        public async Task<IActionResult> StartSurvey(int surveyId)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = await _organisationSurveyService.StartSurvey(surveyId, organisationId);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("startPublicSurvey/{surveyId}")]
        public async Task<IActionResult> StartPublicSurvey(int surveyId)
        {
            var result = await _organisationSurveyService.StartPublicSurvey(surveyId);

            return Ok(result);
        }
        [HttpPost("addAnswersQuestion")]
        public async Task<IActionResult> AddAnswersQuestion(AnswersQuestionViewModels answers)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            await _organisationSurveyService.AddAnswersQuestion(answers, organisationId);

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("addPublicAnswersQuestion")]
        public async Task<IActionResult> AddPublicAnswersQuestion(AnswersQuestionViewModels answers)
        {
            await _organisationSurveyService.AddAnswersQuestion(answers, null);

            return Ok();
        }
        [HttpPost("submitSurvey")]
        public async Task<IActionResult> SubmitSurvey(int organisationSurveyId)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            await _organisationSurveyService.SubmitSurvey(organisationSurveyId, organisationId);

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("submitPublicSurvey")]
        public async Task<IActionResult> SubmitPublicSurvey(int organisationSurveyId)
        {
            await _organisationSurveyService.SubmitSurvey(organisationSurveyId, null);

            return Ok();
        }
        [HttpPost("getOrganizationSurveys")]
        public IActionResult GetOrganizationSurveys(PagingFilterViewModel filter)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = _organisationSurveyService.GetOrganizationSurveys(filter, organisationId);

            return Ok(result);
        }
        [HttpGet("getSectionProgress")]
        public async Task<IActionResult> GetSectionProgress(int organizationSurveyId)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = await _organisationSurveyService.GetSectionProgress(organizationSurveyId, organisationId);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getPublicSectionProgress")]
        public async Task<IActionResult> GetPublicSectionProgress(int organizationSurveyId)
        {
            var result = await _organisationSurveyService.GetSectionProgress(organizationSurveyId, null);

            return Ok(result);
        }
        [HttpGet("getOrganizationSurvey")]
        public IActionResult GetOrganizationSurvey(int organizationSurveyId)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = _organisationSurveyService.GetOrganizationSurvey(organizationSurveyId, organisationId);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getOrganizationPublicSurvey")]
        public async Task<IActionResult> GetOrganizationPublicSurvey(int organizationSurveyId)
        {
            var result = await _organisationSurveyService.GetOrganizationSurvey(organizationSurveyId, false);

            return Ok(result);
        }
        [HttpGet("getScoreSurveyReports")]
        public async Task<IActionResult> GetScoreSurveyReports(int organisationSurveyId)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = await _organisationSurveyService.GetScoreSurveyReports(organisationSurveyId, organisationId);

            return Ok(result);
        }
        [HttpGet("getScorePublicSurveyReports")]
        public async Task<IActionResult> GetScorePublicSurveyReports(int organisationSurveyId)
        {
            var result = await _organisationSurveyService.GetScoreSurveyReports(organisationSurveyId, null);

            return Ok(result);
        }
        [HttpGet("getHistorySurvey")]
        public async Task<IActionResult> GetHistorySurvey(int surveyId)
        {
            var organisationId = Convert.ToInt32(User.GetOrganisationID());
            var result = await _organisationSurveyService.GetHistorySurvey(surveyId, organisationId);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getSurveyAttachments")]
        public IActionResult GetSurveyAttachments(int surveyId)
        {
            var result = _surveyService.GetSurveyAttachments(surveyId);

            return Ok(result);
        }

    }
}
