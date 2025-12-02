using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class OrganisationSector
{
    public int Id { get; set; }

    public int ActivitySectorId { get; set; }

    public int OrganisationId { get; set; }

    public virtual ActivitySector ActivitySector { get; set; } = null!;

    public virtual Organisation Organisation { get; set; } = null!;
}
