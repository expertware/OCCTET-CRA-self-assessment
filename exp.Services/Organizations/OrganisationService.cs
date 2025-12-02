using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.ActivitySectors;
using exp.Infrastructure.Repositories.Countries;
using exp.Infrastructure.Repositories.Organisations;
using exp.Models.Helpers;
using exp.Models.ViewModels;
using exp.Services.Email;
using exp.Services.Users;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace exp.Services.Organisations
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganizationRepository _organisationRepository;
        private readonly IEmailService _emailService;
        private readonly IActivitySectorRepository _activitySectorRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly VaultSettings _vaultSettings;
        private readonly IDataProtector _dataProtector;
        private readonly IUserService _userService;

        public OrganisationService(IOrganizationRepository organizationRepository,
             IEmailService emailService,
             IActivitySectorRepository activitySectorRepository,
             ICountryRepository countryRepository,
             VaultSettings vaultSettings,
             IDataProtectionProvider dataProtectionProvider,
             IUserService userService)
        {
            _organisationRepository = organizationRepository;
            _emailService = emailService;
            _activitySectorRepository = activitySectorRepository;
            _countryRepository = countryRepository;
            _vaultSettings = vaultSettings;
            _dataProtector = dataProtectionProvider.CreateProtector("DataProtectionKey");
            _userService = userService;

        }

        #region Create
        public async Task<(int, string)> RegisterOrganisation(RegisterRequestOrganisationViewModel organisation, string accessCode, string accountId)
        {
            var existOrganisation = _organisationRepository.GetAllQuerable().FirstOrDefault(x => x.Vat == organisation.Vat && x.ConfirmRegister == true);
            if (existOrganisation != null && existOrganisation.ConfirmEmail == true)
            {
                throw new ClientException("Organisation already exists!");
            }
            if (existOrganisation != null && (existOrganisation.ConfirmRegister != true))
            {
                throw new Exception("Organisation request is in pending!");
            }
            var newOrganisation = new Organisation
            {
                Name = organisation.Name,
                Vat = organisation.Vat,
                Domain = organisation.Domain,
                CountryId = organisation.CountryId,
                ContactPersonEmail = _dataProtector.Protect(organisation.ContactEmail),
                ContactPersonName = _dataProtector.Protect(organisation.ContactName),
                ContactPersonRole = organisation.ContactPersonRole,
                AccessCode = _dataProtector.Protect(accessCode),
                ConfirmRegister = false,
                CompanySize = organisation.CompanySize,
                CreatedAt = DateTime.Now,
                AccountId = accountId
            };
            string status;
            var appUrl = _vaultSettings.ApplicationUrl;

            var createdOrganization = await _organisationRepository.Add(newOrganisation);
            if (!DoDomainsMatch(organisation.Domain, organisation.ContactEmail))
            {
                createdOrganization.ConfirmRegister = false;
                createdOrganization.ConfirmEmail = false;
                status = "Pending";
                try
                {
                    _emailService.SendMailRegister(true, organisation.ContactEmail, organisation.Vat, organisation.Name);

                }
                catch
                {
                    await _organisationRepository.Delete(createdOrganization.Id);
                }
                var emails = _userService.GetRequestApproverEmail();
                foreach (var email in emails)
                {
                    _emailService.NotifyAdmin(email, organisation.Vat, organisation.Name, appUrl);
                }
            }
            else
            {
                createdOrganization.ConfirmRegister = true;
                createdOrganization.ConfirmEmailCode = Guid.NewGuid().ToString();
                status = "ConfirmEmail";
                try
                {
                    _emailService.SendMailRegister(false, organisation.ContactEmail, organisation.Vat, organisation.Name, appUrl, createdOrganization.ConfirmEmailCode);
                }
                catch
                {
                    await _organisationRepository.Delete(createdOrganization.Id);
                }
            }
            createdOrganization.DeliveryCountries = organisation.DeliveryCountries.Select(x => new DeliveryCountry
            {
                CountryId = x,
                OrganisationId = createdOrganization.Id
            }).ToList();
            createdOrganization.OrganisationSectors = organisation.ActivitySectors.Select(x => new OrganisationSector
            {
                ActivitySectorId = x,
                OrganisationId = createdOrganization.Id
            }).ToList();

            await _organisationRepository.Update(createdOrganization);

            return (createdOrganization.Id, status);
        }
        //public static bool DoDomainsMatch(string url, string email)
        //{
        //    if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        //    {
        //        url = "http://" + url;
        //    }
        //    Uri uri = new Uri(url);
        //    string urlHost = uri.Host.ToLower();
        //    string[] urlParts = urlHost.Split('.');
        //    string urlDomain = string.Join(".", urlParts[^2], urlParts[^1]);

        //    MailAddress mailAddress = new MailAddress(email);
        //    string emailHost = mailAddress.Host.ToLower();

        //    return urlDomain == emailHost;
        //}
        public static bool DoDomainsMatch(string url, string email)
        {
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(email))
                return false;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;

            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch
            {
                return false; // URL invalid
            }

            MailAddress mailAddress;
            try
            {
                mailAddress = new MailAddress(email);
            }
            catch
            {
                return false; // Email invalid
            }

            string urlHost = uri.Host.ToLower().Trim();
            string emailHost = mailAddress.Host.ToLower().Trim();
            if (urlHost.StartsWith("www."))
                urlHost = urlHost.Substring(4); 

            return urlHost == emailHost || emailHost.EndsWith("." + urlHost);
        }

        public async Task<string> ConfirmEmail(string code)
        {
            var organisation = _organisationRepository.GetAllQuerable().FirstOrDefault(x => x.ConfirmEmailCode == code);

            if (organisation == null)
            {
                throw new Exception("Request not found!");
            }

            if (organisation.ConfirmEmail == true)
            {
                throw new ClientException("Organisation already confirmed!");
            }
            var appUrl = _vaultSettings.ApplicationUrl;
            organisation!.ConfirmEmail = true;

            await _organisationRepository.Update(organisation);
            string organizationName = System.Net.WebUtility.HtmlEncode(organisation.Name);

            var bodyText = $"Here is your access code for {organizationName}:";
            _emailService.SendEmailAccessCode(_dataProtector.Unprotect(organisation.ContactPersonEmail), appUrl!, _dataProtector.Unprotect(organisation.AccessCode), bodyText, organisation.Vat, organisation.Name);

            return _dataProtector.Unprotect(organisation.AccessCode);
        }
        public void ResendCode(string code)
        {
            var organisation = _organisationRepository.GetAllQuerable().FirstOrDefault(x => x.ConfirmEmailCode == code);

            if (organisation == null)
            {
                throw new NotFoundException("Organisation not found!");
            }

            var appUrl = _vaultSettings.ApplicationUrl;

            string organizationName = System.Net.WebUtility.HtmlEncode(organisation.Name);

            var bodyText = $"Here is your access code for {organizationName}:";

            _emailService.SendEmailAccessCode(_dataProtector.Unprotect(organisation.ContactPersonEmail), appUrl!, _dataProtector.Unprotect(organisation.AccessCode), bodyText, organisation.Vat, organisation.Name);
        }

        public void SendEmailToOwner(string organizationVat, string memberRequestEmail, string memberRequestName)
        {
            var existOrganisation = _organisationRepository.GetAllQuerable().FirstOrDefault(x => x.Vat == organizationVat && x.ConfirmEmail == true);

            if (existOrganisation == null)
            {
                throw new NotFoundException("Organisation not found!");
            }

            var appUrl = _vaultSettings.ApplicationUrl;

            _emailService.SendRequestForCodeEmail(_dataProtector.Unprotect(existOrganisation.ContactPersonEmail), memberRequestEmail, existOrganisation.Name, existOrganisation.Vat, appUrl!);

        }
        public void AcceptMemberRequest(string memberRequestEmail, string vat, string accessCode)
        {
            var existOrganization = _organisationRepository.GetAllQuerable().FirstOrDefault(x => x.Vat == vat && x.ConfirmEmail == true);

            if (existOrganization == null)
            {
                throw new NotFoundException("Organisation not found!");
            }
            if (accessCode != _dataProtector.Unprotect(existOrganization!.AccessCode))
            {
                throw new ForbiddenException();
            }

            var appUrl = _vaultSettings.ApplicationUrl;

            var bodyText = $"The owner has been granted access to {existOrganization.Name}. Here is your access code:";
            _emailService.SendEmailAccessCode(memberRequestEmail, appUrl!, _dataProtector.Unprotect(existOrganization.AccessCode), bodyText, existOrganization.Vat, existOrganization.Name);

        }
        public void ContactUs(ContactUsViewModel message)
        {
            var contactEmail = _vaultSettings.EmailContact;
            var messageName = message.Name;
            var messageEmail = message.Email;
            var messageMessage = message.Message;
            string html = $@"
                        <p>Hello,</p>
                        <p>You have received a new message from {messageName} ({messageEmail}):</p>
                        <p>{System.Net.WebUtility.HtmlEncode(messageMessage).Replace("\n", "<br>")}</p>";

            _emailService.Send(contactEmail!, "Contact message", html);
        }
        #endregion

        #region Read
        public List<ActivitySectorViewModel> GetActivitySectorsDetails(string? searchText)
        {
            var sectors = _activitySectorRepository.GetAll().Where(x => x.ActivitySectorId == null);

            if (!String.IsNullOrEmpty(searchText))
            {
                sectors = sectors.Where(x => x.Name.ToLower().Contains(searchText.ToLower()) || x.InverseActivitySectorNavigation.Any(y => y.Name.ToLower().Contains(searchText.ToLower())));
            }
            return sectors.Select(x => new ActivitySectorViewModel
            {
                Name = x.Name,
                SubSector = x.InverseActivitySectorNavigation.Select(y => new ActivitySectorViewModel
                {
                    Name = y.Name,
                    Description = y.Exemples
                }).ToList()
            }).ToList();
        }
        public List<BaseViewModel> GetActivitySectors()
        {
            var sectors = _activitySectorRepository.GetAllQuerable().Where(x => x.ActivitySectorId == null);

            return sectors.Select(x => new BaseViewModel
            {
                Name = x.Name,
                Id = x.Id,
            }).ToList();
        }
        public async Task<List<BaseViewModel>> GetDeliveryCountries(string? searchText)
        {
            var countries = _countryRepository.GetAllQuerable().Where(x => x.IsEuropean == true);

            if (!String.IsNullOrEmpty(searchText))
            {
                countries = countries.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
            }

            return await countries.OrderBy(x => x.Name).Select(x => new BaseViewModel
            {
                Name = x.Name,
                Id = x.Id,
            }).ToListAsync();
        }
        public async Task<List<BaseViewModel>> GetCountries()
        {
            var countries = _countryRepository.GetAllQuerable();

            return await countries.OrderBy(x => x.Name).Select(x => new BaseViewModel
            {
                Name = x.Name,
                Id = x.Id,
            }).ToListAsync();
        }

        public Organisation ExistOrganisationByVat(string vat)
        {
            var organization = _organisationRepository.GetAllOrganizations().FirstOrDefault(x =>x.Vat == vat && x.ConfirmEmail == true);
            if (organization == null)
            {
                throw new NotFoundException("Organization not found");
            }

            return organization;
        }
        public Organisation ExistOrganisation(string accessCode)
        {
            var organization = _organisationRepository.GetAllOrganizations().AsEnumerable().FirstOrDefault(x => _dataProtector.Unprotect(x.AccessCode) == accessCode && x.ConfirmEmail == true);
            if (organization == null)
            {
                throw new NotFoundException("Organization not found");
            }

            return organization;
        }
        public Organisation ExistAccountOrganisation(string accountId)
        {
            var organization = _organisationRepository.GetAllOrganizations().AsEnumerable().FirstOrDefault(x => x.AccountId == accountId && x.ConfirmEmail == true);
            if (organization == null)
            {
              return new Organisation();
            }

            return organization;
        }
        public OrganisationDetailsViewModel GetOrgansation(int organisationId)
        {
            var organization = _organisationRepository.GetAllOrganizations().FirstOrDefault(x => x.Id == organisationId && x.ConfirmEmail == true);

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
                Vat = organization.Vat,
                ContactName = _dataProtector.Unprotect(organization.ContactPersonName),
                ContactPersonRole = organization.ContactPersonRole,
            };

            return organizationDetails;
        }
        #endregion

        #region Update
        public async Task<string> ResetAccessCode(string vat, string accessCode)
        {
            var organization = _organisationRepository.GetAllOrganizations().FirstOrDefault(x => x.Vat == vat && x.ConfirmEmail == true);
            if (organization == null)
            {
                throw new NotFoundException("Organization not found");
            }
            var oldAccessCode = _dataProtector.Unprotect(organization.AccessCode);
            organization.AccessCode = _dataProtector.Protect(accessCode);   

            await _organisationRepository.Update(organization);

            return oldAccessCode;
        }
        public async Task Update(UpdateOrganisationViewModel organization, int organisationId)
        {
            var oldOrganization = _organisationRepository.GetAllOrganizations().FirstOrDefault(x => x.Id == organisationId && x.ConfirmEmail == true);

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

            await _organisationRepository.Update(oldOrganization);
        }
        #endregion
    }
}
