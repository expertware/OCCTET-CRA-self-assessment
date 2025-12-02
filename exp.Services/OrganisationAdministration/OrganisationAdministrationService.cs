using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Organisations;
using exp.Models.Helpers;
using exp.Models.ViewModels;
using exp.Services.Email;
using Microsoft.AspNetCore.DataProtection;

namespace exp.Services.OrganisationAdministrationService
{
    public class OrganisationAdministrationService : IOrganisationAdministrationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IDataProtector _dataProtector;
        private readonly IEmailService _emailService;
        private readonly VaultSettings _vaultSettings;
        public OrganisationAdministrationService(IOrganizationRepository organizationRepository, IDataProtectionProvider dataProtectionProvider, IEmailService emailService, VaultSettings vaultSettings)
        {
            _organizationRepository = organizationRepository;
            _emailService = emailService;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectionKey");
            _vaultSettings = vaultSettings;
        }

        #region Read
        public PagingViewModel<OrganisationsDetailsViewModel> GetOrganisations(FilterCompanies filter)
        {
            var organisations = _organizationRepository.GetAll().Where(x => x.ConfirmRegister == true && x.ConfirmEmail == true).AsEnumerable();

            if (!organisations.Any())
            {
                return new PagingViewModel<OrganisationsDetailsViewModel>();
            }
            if (!String.IsNullOrEmpty(filter.SearchText))
            {
                organisations = organisations.Where(x => x.Name.ToLower().Contains(filter.SearchText.ToLower()) || x.Vat.ToLower().StartsWith(filter.SearchText.ToLower()));
            }
            if (filter.CountryId != null && filter.CountryId != 0)
            {
                organisations = organisations.Where(x => x.CountryId == filter.CountryId);
            }
            if (!String.IsNullOrEmpty(filter.CompanySize))
            {
                organisations = organisations.Where(x => x.CompanySize == filter.CompanySize);
            }

            PagingViewModel<OrganisationsDetailsViewModel> paging = new();

            paging.Count = organisations.Count();
            organisations = filter.OrderBy switch
            {
                "Name" => organisations.OrderBy(x => x.Name),
                "Name_desc" => organisations.OrderByDescending(x => x.Name),
                "RegistrationDate" => organisations.OrderBy(x => x.CreatedAt),
                "RegistrationDate_desc" => organisations.OrderByDescending(x => x.CreatedAt),
                _ => organisations.OrderByDescending(x => x.CreatedAt),
            };

            paging.Items = organisations.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).Select(x => new OrganisationsDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                Vat = x.Vat,
                CompanySize = x.CompanySize,
                Country = x.Country.Name
            }).ToList();

            paging.NumberOfPages = paging.Count / filter.PageSize;
            if (paging.Count % filter.PageSize > 0)
            {
                paging.NumberOfPages += 1;
            }

            return paging;
        }
        public PagingViewModel<RequestOrganisationDetailsViewModel> GetRequestsOrganisations(FilterCompanies filter)
        {
            var organisations = _organizationRepository.GetAllOrganizations().Where(x => x.ConfirmRegister != true).AsEnumerable();

            if (!organisations.Any())
            {
                return new PagingViewModel<RequestOrganisationDetailsViewModel>();
            }
            if (!String.IsNullOrEmpty(filter.SearchText))
            {
                organisations = organisations.Where(x => x.Name.ToLower().StartsWith(filter.SearchText.ToLower()) || x.Vat.ToLower().StartsWith(filter.SearchText.ToLower()));
            }
            if (filter.CountryId != null && filter.CountryId != 0)
            {
                organisations = organisations.Where(x => x.CountryId == filter.CountryId);
            }
            PagingViewModel<RequestOrganisationDetailsViewModel> paging = new();

            paging.Count = organisations.Count();
            organisations = filter.OrderBy switch
            {
                "Name" => organisations.OrderBy(x => x.Name),
                "Name_desc" => organisations.OrderByDescending(x => x.Name),
                "RegistrationDate" => organisations.OrderBy(x => x.CreatedAt),
                "RegistrationDate_desc" => organisations.OrderByDescending(x => x.CreatedAt),
                _ => organisations.OrderBy(x => x.CreatedAt),
            };

            paging.Items = organisations.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).Select(x => new RequestOrganisationDetailsViewModel
            {
                Id = x.Id,
                Name = x.Name,
                RequestDate = x.CreatedAt,
                ContactEmail = _dataProtector.Unprotect(x.ContactPersonEmail),
                ContactName = _dataProtector.Unprotect(x.ContactPersonName),
                Vat = x.Vat,
                Country = x.Country.Name,
                UrlDomain = x.Domain
            }).ToList();

            paging.NumberOfPages = paging.Count / filter.PageSize;
            if (paging.Count % filter.PageSize > 0)
            {
                paging.NumberOfPages += 1;
            }

            return paging;
        }
        public OrganisationDetailsViewModel GetOrgansation(int id)
        {
            var organization = _organizationRepository.GetAllOrganizations().FirstOrDefault(x => x.Id == id && x.ConfirmEmail == true && x.ConfirmRegister == true);

            if (organization == null)
            {
                throw new NotFoundException("Organization not found");
            }

            var organizationDetails = new OrganisationDetailsViewModel
            {
                Id = organization.Id,
                Country = new BaseViewModel
                {
                    Id = organization.CountryId,
                    Name = organization.Country.Name
                },
                CompanySize = organization.CompanySize,
                Domain = organization.Domain,
                Name = organization.Name,
                Vat = organization.Vat,
                ActivitySectors = organization.OrganisationSectors.Select(x => new BaseViewModel
                {
                    Id = x.ActivitySector.Id,
                    Name = x.ActivitySector.Name
                }).ToList(),
                DeliveryCountries = organization.DeliveryCountries.Select(x => new BaseViewModel
                {
                    Id = x.Country.Id,
                    Name = x.Country.Name
                }).ToList(),
                ContactEmail = _dataProtector.Unprotect(organization.ContactPersonEmail),
                ContactName = _dataProtector.Unprotect(organization.ContactPersonName),
                ContactPersonRole = organization.ContactPersonRole
            };

            return organizationDetails;
        }
        public OrganisationDetailsViewModel GetRequestOrgansation(int id)
        {
            var organization = _organizationRepository.GetAllOrganizations().FirstOrDefault(x => x.Id == id);

            if (organization == null)
            {
                throw new NotFoundException("Organization not found");
            }

            var organizationDetails = new OrganisationDetailsViewModel
            {
                Id = organization.Id,
                Country = new BaseViewModel
                {
                    Id = organization.CountryId,
                    Name = organization.Country.Name
                },
                CompanySize = organization.CompanySize,
                Domain = organization.Domain,
                Name = organization.Name,
                Vat = organization.Vat,
                ActivitySectors = organization.OrganisationSectors.Select(x => new BaseViewModel
                {
                    Id = x.ActivitySector.Id,
                    Name = x.ActivitySector.Name
                }).ToList(),
                DeliveryCountries = organization.DeliveryCountries.Select(x => new BaseViewModel
                {
                    Id = x.Country.Id,
                    Name = x.Country.Name
                }).ToList(),
                ContactEmail = _dataProtector.Unprotect(organization.ContactPersonEmail),
                ContactName = _dataProtector.Unprotect(organization.ContactPersonName),
                ContactPersonRole = organization.ContactPersonRole
            };

            return organizationDetails;
        }
        public async Task ResendCodeAdm(int organisationId)
        {
            var organisation = _organizationRepository.GetAllQuerable().FirstOrDefault(x => x.Id == organisationId);

            if (organisation == null)
            {
                throw new NotFoundException("Organisation not found!");
            }

            var appUrl = _vaultSettings.ApplicationUrl;

            await _organizationRepository.Update(organisation);
            string organizationName = System.Net.WebUtility.HtmlEncode(organisation.Name);

            var bodyText = $"Here is your access code for {organizationName}:";
            _emailService.SendEmailAccessCode(_dataProtector.Unprotect(organisation.ContactPersonEmail), appUrl!, _dataProtector.Unprotect(organisation.AccessCode), bodyText, organisation.Vat, organisation.Name);

        }
        #endregion

        #region Create
        public async Task RespondRequest(RespondeRequestViewModel response)
        {
            var organisation = await _organizationRepository.Get(response.OrganisationId);
            var appUrl = _vaultSettings.ApplicationUrl;

            if (organisation == null)
            {
                throw new NotFoundException("Not found!");
            }
            if (response.IsAprrove == false)
            {
                await _organizationRepository.DeleteAsync(organisation);
                _emailService.RejectedRequest(_dataProtector.Unprotect(organisation.ContactPersonEmail), appUrl!, organisation.Vat, organisation.Name);
                return;
            }
            organisation.ConfirmEmail = response.IsAprrove;
            organisation.ConfirmRegister = response.IsAprrove;
            //organisation.AccessCode = _dataProtector.Protect( Guid.NewGuid().ToString());

            await _organizationRepository.Update(organisation);
            string organizationName = System.Net.WebUtility.HtmlEncode(organisation.Name);

            var bodyText = $"Here is your access code for {organizationName}:";
            _emailService.SendEmailAccessCode(_dataProtector.Unprotect(organisation.ContactPersonEmail), appUrl!, _dataProtector.Unprotect(organisation.AccessCode), bodyText, organisation.Vat, organisation.Name);
        }
        #endregion

        #region Delete
        public async Task Delete(int organisationId)
        {
            var organisation =  _organizationRepository.GetDetails().Where(x => x.Id == organisationId).FirstOrDefault();

            if (organisation == null)
            {
                throw new Exception("Not found!");
            }

            organisation.IsDeleted = true;
            organisation.UpdatedAt = DateTime.Now;
            organisation.Account.IsDeleted = true;
            await _organizationRepository.Update(organisation);
        }
        #endregion

        #region Update
        public async Task Update(UpdateOrganisationAdmViewModel organization)
        {
            var oldOrganization = _organizationRepository.GetAllOrganizations().FirstOrDefault(x => x.Id == organization.Id && x.ConfirmEmail == true);

            if (oldOrganization == null)
            {
                {
                    throw new Exception("Something went wrong!");
                }
            }

            oldOrganization.CountryId = organization.CountryId;
            oldOrganization.Domain = organization.Domain;
            oldOrganization.Name = organization.Name;
            oldOrganization.DeliveryCountries.Clear();
            oldOrganization.UpdatedAt = DateTime.Now;
            oldOrganization.CompanySize = organization.CompanySize;
            oldOrganization.ContactPersonEmail = _dataProtector.Protect(organization.ContactEmail);
            oldOrganization.ContactPersonName = _dataProtector.Protect(organization.ContactName);
            oldOrganization.ContactPersonRole = organization.ContactPersonRole;
            oldOrganization.DeliveryCountries = organization.DeliveryCountries.Select(x => new DeliveryCountry
            {
                CountryId = x,
                OrganisationId = oldOrganization.Id,
            }).ToList();

            oldOrganization.OrganisationSectors.Clear();
            oldOrganization.OrganisationSectors = organization.ActivitySectors.Select(x => new OrganisationSector
            {
                ActivitySectorId = x,
                OrganisationId = oldOrganization.Id,
            }).ToList();

            await _organizationRepository.Update(oldOrganization);
        }
        #endregion
    }
}
