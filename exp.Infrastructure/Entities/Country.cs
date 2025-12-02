using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsEuropean { get; set; }

    public virtual ICollection<DeliveryCountry> DeliveryCountries { get; set; } = new List<DeliveryCountry>();

    public virtual ICollection<Organisation> Organisations { get; set; } = new List<Organisation>();
}
