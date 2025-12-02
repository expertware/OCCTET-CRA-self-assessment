using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Attachments;
using exp.Infrastructure.Repositories.Surveys;
using exp.Models.ViewModels;

namespace exp.Services.Surveys
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IQuestionAttachments _questionAttachments;

        public SurveyService(ISurveyRepository surveyRepository, IQuestionAttachments questionAttachments)
        {
            _surveyRepository = surveyRepository;
            _questionAttachments = questionAttachments;
        }

        #region Create

        #endregion

        #region Read
        public List<GroupedSurveyDetailsViewModel> GetSurveys()
        {
            return _surveyRepository.GetAllQuerable().Where(x => x.IsPublished == true).Select(x => new GroupedSurveyDetailsViewModel
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name
            }).ToList();
        }
        public Survey GetSurveyDetails(int surveyId)
        {
            var survey = _surveyRepository.GetAll().Where(x => x.Id == surveyId && x.IsPublished == true).FirstOrDefault();

            if (survey == null)
            {
                return new Survey();
            }
            return survey;
        }
        public async Task<GroupedSurveyDetailsViewModel> GetSurvey(int surveyId)
        {
            var survey = await _surveyRepository.Get(surveyId);

            if (survey == null && survey.IsPublished == true)
            {
                return new GroupedSurveyDetailsViewModel();
            }

            var surveyDetails = new GroupedSurveyDetailsViewModel
            {
                Id = survey.Id,
                Description = survey.Description,
                Name = survey.Name
            };

            return surveyDetails;
        }
        //public List<SurveyResponseDetailsViewModel> GetAllSurveys()
        //{
        //    return _surveyRepository.GetAll().Select(x => new SurveyResponseDetailsViewModel
        //    {
        //        Id = x.Id,
        //        Description = x.Description,
        //        Title = x.Name,
        //         Sections = x.SurveySections.Select(y => new SectionDetailsViewModel
        //         {
        //              Id = y.Id,   
        //               Title = y.Name,
        //                Description = y.Description,
        //                  SubSection = y.ParentSection.Name,

        //         })
        //    }).ToList();
        //}
        public async Task<Survey> SurveyExist(int surveyId)
        {
            var survey = await _surveyRepository.Get(surveyId);

            if (survey == null || survey.IsPublished != true)
            {
                throw new Exception("Survey not found");
            }

            return survey;
        }
        public List<AttachmentsDetailsViewModel> GetSurveyAttachments(int surveyId)
        {
            var attachments = _questionAttachments.GetAttachments().Where(x => x.Question.Section.SurveyId == surveyId).Select(x => x.Attachment).Distinct().ToList();

            if (!attachments.Any())
            {
                return new List<AttachmentsDetailsViewModel> { };
            }

            var attachmentsDetails = attachments.Select(x => new AttachmentsDetailsViewModel
            {
                Id = x.Id,
                Content = x.Content,
                Name = x.Name
            }).ToList();

            return attachmentsDetails;
        }

        #endregion
    }
}
