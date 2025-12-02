using exp.Models.ViewModels;

namespace exp.Services.Users
{
    public interface IUserService
    {
        PagingViewModel<UserDetailsViewModel> GetUsers(FilterUsers filter);
        UserDetailsForEditViewModel GetUserForEdit(string userId);
        List<RoleDetailsViewModel> GetRoles();
        Task Update(UpdateUserDetailsViewModel userDetails);
        Task Delete(string userId);
        List<string> GetRequestApproverEmail();
    }
}
