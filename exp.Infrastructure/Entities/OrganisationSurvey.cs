using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class OrganisationSurvey
{
    public int Id { get; set; }

    public int? OrganisationId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int SurveyId { get; set; }

    public string? Result { get; set; }

    public virtual Organisation? Organisation { get; set; }

    public virtual Survey Survey { get; set; } = null!;

    public virtual ICollection<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();
}
