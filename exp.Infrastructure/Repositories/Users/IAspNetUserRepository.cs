using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.Users
{
    public interface IAspNetUserRepository : IGenericRepository<AspNetUser>
    {
        public IQueryable<AspNetUser> GetAll();
    }
}
