using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class VwOrganisationResult
{
    public int? Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? OrganisationId { get; set; }

    public string? OrganisationName { get; set; }

    public int? SurveyId { get; set; }

    public string? Result { get; set; }

    public int? CountryId { get; set; }

    public string? CountryName { get; set; }

    public string? CompanySize { get; set; }

    public int? ActivitySectorId { get; set; }

    public string? ActivitySectorName { get; set; }
}
