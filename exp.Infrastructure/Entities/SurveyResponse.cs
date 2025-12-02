using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class SurveyResponse
{
    public int Id { get; set; }

    public int OrganisationSurveyId { get; set; }

    public int QuestionId { get; set; }

    public string Response { get; set; } = null!;

    public string? Mentions { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual OrganisationSurvey OrganisationSurvey { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
