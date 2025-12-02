using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class VwOrgannisationsReport
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public string? CompanySize { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CountryId { get; set; }

    public string? CountryName { get; set; }

    public int? ActivitySectorId { get; set; }

    public string? ActivitySectorName { get; set; }
}
