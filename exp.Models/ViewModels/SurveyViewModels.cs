using System.ComponentModel.DataAnnotations;

namespace exp.Models.ViewModels
{
    public class CreateSurveyViewModel
    {
        [Required]
        [StringLength(500)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public required int SurveyTypeId { get; set; }
    }
    public class GroupedSurveyDetailsViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int NumberOfFinishedSurveys { get; set; }
        public int? ProgressedSurveyId { get; set; }
    }
    public class SurveyDetailsViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<HistorySurveyDetailsViewModel> Surveys { get; set; } = default!;
    }
    public class MaturityReportsViewModel
    {
        public List<ResultReportsViewModel> DefaultResult { get; set; }
        public List<ResultReportsViewModel> OrganisationResult { get; set; }
        public string? TotalScore { get; set; } = default!;

    }
    public class ResultReportsViewModel
    {
        public string Result { get; set; } = default!;
        public string Category { get; set; } = default!;

    }
    public class HistorySurveyDetailsViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Status { get; set; } = default!;
        public string? Result { get; set; }
        public int? OrganisationSurveyId { get; set; }
    }
    public class AttachmentsDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
   
    public class FilterPagingSurveyAdmViewModel : PagingFilterViewModel
    {
        public required int OrganisationId { get; set; }
    }
}
