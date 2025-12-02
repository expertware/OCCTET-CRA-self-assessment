using exp.Models.ViewModels;

namespace exp.Services.Questions
{
    public interface ISurveyContentService
    {
        Task<int> CreateQuestion(CreateQuestionViewModel questionViewModel, string userId);
        Task<int> CreateSurvey(CreateSurveyViewModel survey, string userId);
        Task<int> CreateSection(CreateSectionViewModel section, string userId);
    }
}
