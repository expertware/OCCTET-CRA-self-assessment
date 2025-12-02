using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class DeliveryCountry
{
    public int Id { get; set; }

    public int CountryId { get; set; }

    public int OrganisationId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Organisation Organisation { get; set; } = null!;
}
