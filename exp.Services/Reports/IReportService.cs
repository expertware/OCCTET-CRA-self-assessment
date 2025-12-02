using exp.Models.ViewModels;

namespace exp.Services.Reports
{
    public interface IReportService
    {
        List<BaseReportViewModel> GetOrganisationsOnCountries(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetCountOrganisationsByCRAScope(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetOrganizationsRgistersOnTime(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetOrganizationsBySize(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetTopRegisteredSectors(ReportFilterViewModel filter);
        MaturityReportsViewModel GetMaturityCategories(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetAwarenessofTheCRA(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetOrganisationCRARegulationApply(ReportFilterViewModel filter);
        List<BaseReportViewModel> GetAwarenessSurveyReport(ReportFilterViewModel filter);
    }
}
