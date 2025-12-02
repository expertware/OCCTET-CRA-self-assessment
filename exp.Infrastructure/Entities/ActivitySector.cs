using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class ActivitySector
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Exemples { get; set; }

    public int? ActivitySectorId { get; set; }

    public virtual ActivitySector? ActivitySectorNavigation { get; set; }

    public virtual ICollection<ActivitySector> InverseActivitySectorNavigation { get; set; } = new List<ActivitySector>();

    public virtual ICollection<OrganisationSector> OrganisationSectors { get; set; } = new List<OrganisationSector>();
}
