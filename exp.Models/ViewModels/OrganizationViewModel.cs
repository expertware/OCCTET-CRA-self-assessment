using exp.Models.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace exp.Models.ViewModels
{
    public class RegisterRequestOrganisationViewModel
    {
        [Required]
        [StringLength(500, MinimumLength = 3)]
        [Sanitize]
        [DisallowUrl]
        public string Name { get; set; } = default!;
        [Required]
        [Sanitize]
        public string Vat { get; set; } = default!;
        [Required]
        [Url]
        [Sanitize]
        public string Domain { get; set; } = default!;
        [Required]
        public List<int> ActivitySectors { get; set; } = default!;
        [Required]
        [StringLength(500, MinimumLength = 3)]
        [DisallowUrl]
        [Sanitize]
        public string ContactName { get; set; } = default!;
        [Required]
        [StringLength(500)]
        [Sanitize]
        public string ContactEmail { get; set; } = default!;
        [Required]
        [StringLength(200)]
        [DisallowUrl]
        [Sanitize]
        public string ContactPersonRole { get; set; } = default!;
        [Required]
        [StringLength(200, MinimumLength = 3)]
        [Sanitize]
        public string CompanySize { get; set; } = default!;
        [Required]
        public int CountryId { get; set; }
        public int? OrganisationSurveyId { get; set; }
        [Required]
        public required List<int> DeliveryCountries { get; set; }
        [Required]
        public string CaptchaToken { get; set; } = default!;
        [Required]
        public bool Agreement { get; set; } = default!;
    }

    public class OrganisationDetailsViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; } = default!;
        public string Vat { get; set; } = default!;
        public string Domain { get; set; } = default!;
        public BaseViewModel Country { get; set; }
        public string ContactName { get; set; } = default!;
        public string ContactEmail { get; set; } = default!;
        public string CompanySize { get; set; } = default!;
        public string ContactPersonRole { get; set; } = default!;
        public List<BaseViewModel> DeliveryCountries { get; set; } = default!;
        public List<BaseViewModel> ActivitySectors { get; set; } = default!;
    }
    public class RequestOrganisationDetailsViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; } = default!;
        public string Vat { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string UrlDomain { get; set; } = default!;
        public DateTime? RequestDate { get; set; }
        public string ContactEmail { get; set; } = default!;
        public string ContactName { get; set; } = default!;
    }
    public class OrganisationsDetailsViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; } = default!;
        public string Vat { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string CompanySize { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
    }
    public class FilterCompanies : PagingFilterViewModel
    {
        public string? SearchText { get; set; }
        public int? CountryId { get; set; }
        public string? CompanySize { get; set; }
    }
    public class RespondeRequestViewModel
    {
        public int OrganisationId { get; set; }
        public bool IsAprrove { get; set; }
    }
    public class SurveySectionStatusViewModel : BaseViewModel
    {
        public string Status { get; set; }
    }
    public class UpdateOrganisationViewModel
    {
        [Required]
        [DisallowUrl]
        [StringLength(500, MinimumLength = 3)]
        [Sanitize]
        public string Name { get; set; } = default!;
        [Required]
        [Sanitize]
        public string Domain { get; set; } = default!;
        [Required]
        public List<int> ActivitySectors { get; set; } = default!;

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [Sanitize]
        public string CompanySize { get; set; } = default!;
        [Required]
        public int CountryId { get; set; }
        [Required]
        public required List<int> DeliveryCountries { get; set; }
    }
    public class UpdateOrganisationAdmViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisallowUrl]
        [StringLength(500, MinimumLength = 3)]
        [Sanitize]
        public string Name { get; set; } = default!;
        [Required]
        [Sanitize]
        public string Domain { get; set; } = default!;
        [Required]
        public List<int> ActivitySectors { get; set; } = default!;
        [Required]
        [StringLength(500, MinimumLength = 3)]
        [Sanitize]
        public string ContactName { get; set; } = default!;
        [Required]
        [StringLength(500)]
        [Sanitize]
        public string ContactEmail { get; set; } = default!;
        [Required]
        [StringLength(200)]
        [Sanitize]
        public string ContactPersonRole { get; set; } = default!;
        [Required]
        [StringLength(200, MinimumLength = 3)]
        [Sanitize]
        public string CompanySize { get; set; } = default!;
        [Required]
        public int CountryId { get; set; }
        [Required]
        public required List<int> DeliveryCountries { get; set; }
    }

    public class ContactUsViewModel
    {
        [Required]
        [DisallowUrl]
        [StringLength(500, MinimumLength = 3)]
        [Sanitize]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Sanitize]
        public string Email { get; set; } = default!;
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        [Sanitize]
        public string Message { get; set; } = default!;
        [Required]
        public string CaptchaToken { get; set; } = default!;
    }
    public class ActivitySectorViewModel
    {
        [DisallowUrl]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<ActivitySectorViewModel>? SubSector { get; set; }
    }
}
