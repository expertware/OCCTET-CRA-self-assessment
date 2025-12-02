using exp.Models.ViewModels;
using exp.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exp.Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ("Admin,Raports Viewer"))]

    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("getOrganisationsOnCountries")]
        public IActionResult GetOrganisationsOnCountries(ReportFilterViewModel filter)
        {
            var result = _reportService.GetOrganisationsOnCountries(filter);

            return Ok(result);
        }
        [HttpPost("getCountOrganisationsByCRAScope")]
        public IActionResult GetCountOrganisationsByCRAScope(ReportFilterViewModel filter)
        {
            var result = _reportService.GetCountOrganisationsByCRAScope(filter);

            return Ok(result);
        }
       
        [HttpPost("getOrganizationsRgistersOnTime")]
        public IActionResult GetOrganizationsRgistersOnTime(ReportFilterViewModel filter)
        {
            var result = _reportService.GetOrganizationsRgistersOnTime(filter);

            return Ok(result);
        }
        [HttpPost("getOrganizationsBySize")]
        public IActionResult GetOrganizationsBySize(ReportFilterViewModel filter)
        {
            var result = _reportService.GetOrganizationsBySize(filter);

            return Ok(result);
        }
        [HttpPost("getTopRegisteredSectors")]
        public IActionResult GetTopRegisteredSectors(ReportFilterViewModel filter)
        {
            var result = _reportService.GetTopRegisteredSectors(filter);

            return Ok(result);
        }
        [HttpPost("getMaturityCategories")]
        public IActionResult GetMaturityCategories(ReportFilterViewModel filter)
        {
            var result = _reportService.GetMaturityCategories(filter);

            return Ok(result);
        }
        [HttpPost("getAwarenessofTheCRA")]
        public IActionResult GetAwarenessofTheCRA(ReportFilterViewModel filter)
        {
            var result = _reportService.GetAwarenessofTheCRA(filter);

            return Ok(result);
        }
        [HttpPost("getOrganisationCRARegulationApply")]
        public IActionResult GetOrganisationCRARegulationApply(ReportFilterViewModel filter)
        {
            var result = _reportService.GetOrganisationCRARegulationApply(filter);

            return Ok(result);
        }
        [HttpPost("getAwarenessSurveyReport")]
        public IActionResult GetAwarenessurveyReport(ReportFilterViewModel filter)
        {
            var result = _reportService.GetAwarenessSurveyReport(filter);

            return Ok(result);
        }
    }
}
