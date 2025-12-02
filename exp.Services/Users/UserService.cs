using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Roles;
using exp.Infrastructure.Repositories.Users;
using exp.Models.ViewModels;

namespace exp.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IAspNetUserRepository _repository;
        private readonly IRoleRepository _roleRepository;
        public UserService(IAspNetUserRepository repository, IRoleRepository roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }

        #region Read
        public List<string> GetRequestApproverEmail()
        {
            var users = _repository.GetAll().Where(x => x.Email != null && x.AspNetUserRoles.Any(y => y.Role.Name == "Request Approver") && x.Email != null);

            if (!users.Any())
            {
                return new List<string>();
            }

            return users.Select(x => x.Email!).ToList();
        }
        public PagingViewModel<UserDetailsViewModel> GetUsers(FilterUsers filter)
        {
            var users = _repository.GetAllQuerable().Where(x => x.Email != null);

            if (!users.Any())
            {
                return new PagingViewModel<UserDetailsViewModel>();
            }

            if (!String.IsNullOrEmpty(filter.SearchText))
            {
                users = users.Where(x => x.Name.ToLower().Contains(filter.SearchText.ToLower()));
            }
            if (!String.IsNullOrEmpty(filter.RoleId))
            {
                users = users.Where(x => x.AspNetUserRoles.Any(x => x.RoleId == filter.RoleId));
            }
            PagingViewModel<UserDetailsViewModel> paging = new();
            paging.Count = users.Count();
            users = filter.OrderBy switch
            {
                "Name" => users.OrderBy(x => x.Name),
                "Name_desc" => users.OrderByDescending(x => x.Name),
                _ => users.OrderBy(x => x.Name),
            };

            paging.Items = users.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).Select(x => new UserDetailsViewModel
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Roles = x.AspNetUserRoles.Select(y => y.Role.Name).ToList()
            }).ToList();
            paging.NumberOfPages = paging.Count / filter.PageSize;
            if (paging.Count % filter.PageSize > 0)
            {
                paging.NumberOfPages += 1;
            }

            return paging;
        }
        public UserDetailsForEditViewModel GetUserForEdit(string userId)
        {
            var user = _repository.GetAll().Where(x => x.Email != null).FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return new UserDetailsForEditViewModel();
            }
            var userDetails = new UserDetailsForEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Roles = user.AspNetUserRoles.Select(x => new RoleDetailsViewModel
                {
                    Id = x.RoleId,
                    Name = x.Role.Name
                }).ToList()
            };

            return userDetails;
        }
        public List<RoleDetailsViewModel> GetRoles()
        {
            var roles = _roleRepository.GetAllQuerable().Where(x => x.Name != "Guest");

            return roles.Select(x => new RoleDetailsViewModel { Id = x.Id, Name = x.Name }).ToList();
        }
        #endregion
        #region Update 
        public async Task Update(UpdateUserDetailsViewModel userDetails)
        {
            var user = _repository.GetAll().Where(x => x.Email != null).FirstOrDefault(x => x.Id == userDetails.Id);

            if (user == null)
            {
                throw new Exception("Not found!");
            }

            user.Name = userDetails.Name;
            if (userDetails.RoleIds != null && userDetails.RoleIds.Any())
            {
                user.AspNetUserRoles = userDetails.RoleIds.Select(x => new AspNetUserRole
                {
                    RoleId = x,
                    UserId = user.Id
                }).ToList();
            }

            await _repository.Update(user);
        }
        #endregion
        #region Delete 
        public async Task Delete(string userId)
        {
            var user = _repository.GetAll().Where(x => x.Email != null).FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                throw new Exception("Not found!");
            }

            user.IsDeleted = true;


            await _repository.Update(user);
        }
        #endregion
    }
}
