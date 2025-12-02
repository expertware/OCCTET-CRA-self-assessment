using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class Organisation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Vat { get; set; } = null!;

    public int CountryId { get; set; }

    public string AccessCode { get; set; } = null!;

    public string Domain { get; set; } = null!;

    public string CompanySize { get; set; } = null!;

    public string ContactPersonRole { get; set; } = null!;

    public string ContactPersonName { get; set; } = null!;

    public string ContactPersonEmail { get; set; } = null!;

    public int? OrganizationSurveyId { get; set; }

    public bool? ConfirmRegister { get; set; }

    public bool? ConfirmEmail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ConfirmEmailCode { get; set; }

    public string AccountId { get; set; } = null!;

    public virtual AspNetUser Account { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<DeliveryCountry> DeliveryCountries { get; set; } = new List<DeliveryCountry>();

    public virtual ICollection<OrganisationSector> OrganisationSectors { get; set; } = new List<OrganisationSector>();

    public virtual ICollection<OrganisationSurvey> OrganisationSurveys { get; set; } = new List<OrganisationSurvey>();
}
