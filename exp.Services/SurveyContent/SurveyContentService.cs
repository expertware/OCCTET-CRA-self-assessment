using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Attachments;
using exp.Infrastructure.Repositories.Questions;
using exp.Infrastructure.Repositories.Sections;
using exp.Infrastructure.Repositories.Surveys;
using exp.Models.Helpers;
using exp.Models.ViewModels;

namespace exp.Services.Questions
{
    public class SurveyContentService : ISurveyContentService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ISurveyRepository _surveyRepository;
        private readonly ISectionSurveyRepository _sectionSurveyRepository;
        private readonly IQuestionAttachments _questionAttachmentsRepository;

        public SurveyContentService(IQuestionRepository questionRepository
            , ISurveyRepository surveyRepository
            , IQuestionAttachments questionAttachmentsRepository
            , ISectionSurveyRepository sectionSurveyRepository)
        {
            _questionRepository = questionRepository;
            _surveyRepository = surveyRepository;
            _questionAttachmentsRepository = questionAttachmentsRepository;
            _sectionSurveyRepository = sectionSurveyRepository;
        }

        #region Create
        public async Task<int> CreateSurvey(CreateSurveyViewModel survey, string userId)
        {
            var newSurvey = new Survey
            {
                Name = survey.Name,
                Description = survey.Description,
                CrearedBy = userId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsPublished = false,
            };

            await _surveyRepository.Add(newSurvey);

            return newSurvey.Id;
        }
        public async Task<int> CreateSection(CreateSectionViewModel section, string userId)
        {
            var survey = await _surveyRepository.Get(section.SurveyId);

            if (survey.CrearedBy != userId)
            {
                throw new ForbiddenException();
            }

            var newSection = new SurveySection
            {
                Name = section.Name,
                Description = section.Description,
                ParentSectionId = section.ParentSectionId,
                IsDeleted = false,
                SurveyId = section.SurveyId,
            };

            await _sectionSurveyRepository.Add(newSection);

            return newSection.Id;
        }

        public async Task<int> CreateQuestion(CreateQuestionViewModel questionViewModel , string userId)
        {
            var section = await _sectionSurveyRepository.Get(questionViewModel.SectionId);
            var survey = await _surveyRepository.Get(section.SurveyId);
            if (survey.CrearedBy != userId)
            {
                throw new ForbiddenException();
            }

            var newQuestion = new Question
            {
                Name = questionViewModel.Name,
                Description = questionViewModel.Description,
                IsDeleted = false,
                SectionId = questionViewModel.SectionId,
                ParrentAnswerId = questionViewModel.ParentAnswerId,
                QuestionTypeId = questionViewModel.QuestionTypeId,
                Position = questionViewModel.Position, 
                AnswersPrompts = questionViewModel.AnswersList != null ? questionViewModel.AnswersList.Select(x => new AnswersPrompt
                {
                    Answers = x!.Answers,
                    Prompt = x.Prompt,
                    Comment = x.Comment,
                }).ToList() : new List<AnswersPrompt>()
            };

            newQuestion = await _questionRepository.Add(newQuestion);

            if (questionViewModel.Attachments != null && !questionViewModel.Attachments.Any())
            {
                var attachments = questionViewModel.Attachments.Select(x => new QuestionAttachment
                {
                    AttachmentId = x,
                    QuestionId = newQuestion.Id
                }).ToList();
                await _questionAttachmentsRepository.AddRangeAsync(attachments);
            }

            return newQuestion.Id;

        }
        #endregion

        #region Read
       
        #endregion
    }
}
