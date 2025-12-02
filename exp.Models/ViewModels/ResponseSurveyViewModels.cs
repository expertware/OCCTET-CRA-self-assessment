namespace exp.Models.ViewModels
{
    public class AnswersQuestionViewModels
    {
        public int QuestionId { get; set; }
        public int OrganizationSurveyId { get; set; }
        public string? Answer { get; set; } = default!;
        public string? Mentions { get; set; } = default!;
    }

    public class AddAnswerResponseViewModel
    {
        public List<SectionDetailsViewModel>? SubSection { get; set; }
        public List<SectionDetailsViewModel>? Section { get; set; }
        public List<QuestionAnswersViewModel> Questions { get; set; } = default!;
    }
    public class QuestionAnswersViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
        public string? Answer { get; set; }
        public string Type { get; set; } = default!;
        public List<BaseViewModel>? Attachments { get; set; }
        public int? ParentId { get; set; }
    }
    public class AnswerViewModel
    {
        public string? Answer { get; set; }
        public string? Prompt { get; set; }
        public string? Mentions { get; set; }
        public string? Comment { get; set; }
        public bool? Check { get; set; }
    }
    public class SectionDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public List<SectionDetailsViewModel>? SubSection { get; set; }
        public List<QuestionAnswersViewModel> Questions { get; set; } = default!;
    }
    public class SurveyResponseDetailsViewModel
    {
        public int Id { get; set; }
        public int OrganizationSurveyId { get; set; }
        public string Title { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? Description { get; set; }
        public bool? Submited { get; set; }
        public string? Result { get; set; }
        public List<SectionDetailsViewModel> Sections { get; set; } = default!;
    }
}
