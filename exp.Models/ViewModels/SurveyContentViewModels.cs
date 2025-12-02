using System.ComponentModel.DataAnnotations;

namespace exp.Models.ViewModels
{
    public class CreateSectionViewModel
    {
        [Required]
        [StringLength(500)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentSectionId { get; set; }
        [Required]
        public int SurveyId { get; set; }
    }
    public class CreateQuestionViewModel
    {
        [Required]
        [StringLength(500)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public required int QuestionTypeId { get; set; }
        public int? ParentAnswerId { get; set; }
        public List<AnswersPromptViewModel?>? AnswersList { get; set; }
        public List<int>? Attachments { get; set; }
        [Required]
        public int Position { get; set; }
        [Required]
        public int SectionId { get; set; }
    }
    public class AnswersPromptViewModel
    {
        [Required]
        public string Answers { get; set; } = default!;
        public string? Prompt { get; set; }
        public string? Mentions { get; set; }
        public string? Comment { get; set; }
    }
}
