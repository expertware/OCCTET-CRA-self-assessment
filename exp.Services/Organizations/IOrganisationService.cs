using exp.Infrastructure.Entities;
using exp.Models.ViewModels;

namespace exp.Services.Organisations
{
    public interface IOrganisationService
    {
        List<BaseViewModel> GetActivitySectors();
        void ResendCode(string code);
        void ContactUs(ContactUsViewModel message);
        Task<List<BaseViewModel>> GetDeliveryCountries(string? searchText);
        Task<List<BaseViewModel>> GetCountries();
        Task<(int, string)> RegisterOrganisation(RegisterRequestOrganisationViewModel organization, string accessCode, string accountId);
        OrganisationDetailsViewModel GetOrgansation(int organisationId);
        void SendEmailToOwner(string organizationVat, string memberRequestEmail, string memberRequestName);
        Task<string> ConfirmEmail(string code);
        Task Update(UpdateOrganisationViewModel organization, int organisationId);
        Task<string> ResetAccessCode(string vat, string accessCode);
        Organisation ExistOrganisation(string accessCode);
        Organisation ExistAccountOrganisation(string accountId);
        Organisation ExistOrganisationByVat(string vat);
        void AcceptMemberRequest(string memberRequestEmail, string vat, string accessCode);
        List<ActivitySectorViewModel> GetActivitySectorsDetails(string? searchText);
    }
}
