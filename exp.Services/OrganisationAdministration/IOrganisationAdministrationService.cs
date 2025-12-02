using exp.Models.ViewModels;

namespace exp.Services.OrganisationAdministrationService
{
    public interface IOrganisationAdministrationService
    {
        PagingViewModel<RequestOrganisationDetailsViewModel> GetRequestsOrganisations(FilterCompanies filter);
        PagingViewModel<OrganisationsDetailsViewModel> GetOrganisations(FilterCompanies filter);
        OrganisationDetailsViewModel GetOrgansation(int id);
        OrganisationDetailsViewModel GetRequestOrgansation(int id);
        Task RespondRequest(RespondeRequestViewModel response);
        Task Delete(int organisationId);
        Task Update(UpdateOrganisationAdmViewModel organization);
        Task ResendCodeAdm(int organisationId);
    }
}
