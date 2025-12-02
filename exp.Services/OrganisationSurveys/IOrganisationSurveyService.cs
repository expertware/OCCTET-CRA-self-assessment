using exp.Models.ViewModels;

namespace exp.Services.OrganisationSurveys
{
    public interface IOrganisationSurveyService
    {
        Task<int> StartSurvey(int surveyId, int organisationId);
        Task<int> StartPublicSurvey(int surveyId);
        PagingViewModel<GroupedSurveyDetailsViewModel> GetOrganizationSurveys(PagingFilterViewModel filter, int organisationId);
        PagingViewModel<GroupedSurveyDetailsViewModel> GetOrganizationSurveysForAdm(FilterPagingSurveyAdmViewModel filter);
        Task<List<SurveySectionStatusViewModel>> GetSectionProgress(int organisationSurveyId, int? organisationId);
        SurveyResponseDetailsViewModel GetOrganizationSurvey(int organizationSurveyId, int? organisationId);
        Task<SurveyResponseDetailsViewModel> GetOrganizationSurvey(int organizationSurveyId, bool forAdm);
        Task<MaturityReportsViewModel> GetScoreSurveyReports(int organisationSurveyId, int? organisationId);
        MaturityReportsViewModel GetScoreSurveyReports(int organisationSurveyId);
        Task<SurveyDetailsViewModel> GetHistorySurveyAdm(int organisationId, int surveyId);
        Task<SurveyDetailsViewModel> GetHistorySurvey(int surveyId, int organizationId);
        Task AddAnswersQuestion(AnswersQuestionViewModels answers, int? organisationId);
        Task AddOrganizationAtSurvey(int organizationId, int organizationSurveyId);
        Task SubmitSurvey(int organisationSurveyId, int? organisationId);
    }
}
