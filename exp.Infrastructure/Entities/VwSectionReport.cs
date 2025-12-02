using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class VwSectionReport
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? Response { get; set; }

    public int? QuestionId { get; set; }

    public int? OrganisationSurveyId { get; set; }

    public int? SurveyId { get; set; }

    public int? CountryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CompanySize { get; set; }

    public int? ActivitySectorId { get; set; }

    public string? Result { get; set; }
}
