using exp.Infrastructure.Entities;
using exp.Models.ViewModels;

namespace exp.Services.Surveys
{
    public interface ISurveyService
    {
        Task<Survey> SurveyExist(int surveyId);
        Survey GetSurveyDetails(int surveyId);
        List<GroupedSurveyDetailsViewModel> GetSurveys();
        Task<GroupedSurveyDetailsViewModel> GetSurvey(int surveyId);
        List<AttachmentsDetailsViewModel> GetSurveyAttachments(int surveyId);
    }
}
