using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class Survey
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CrearedBy { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsPublished { get; set; }

    public bool? IsPublic { get; set; }

    public string SurveyType { get; set; } = null!;

    public string? EvaluationScript { get; set; }

    public virtual AspNetUser? CrearedByNavigation { get; set; }

    public virtual ICollection<OrganisationSurvey> OrganisationSurveys { get; set; } = new List<OrganisationSurvey>();

    public virtual ICollection<SurveySection> SurveySections { get; set; } = new List<SurveySection>();
}
