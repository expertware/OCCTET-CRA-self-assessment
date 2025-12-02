using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Users
{
    public class AspNetUserRepository : GenericRepository<AspNetUser>, IAspNetUserRepository
    {
        private readonly SurveyDBContext _surveyDBContext;
        public AspNetUserRepository(SurveyDBContext context) : base(context)
        {
            _surveyDBContext = context;
        }

        public IQueryable<AspNetUser> GetAll()
        {

            return _surveyDBContext.AspNetUsers
                .Include(x => x.AspNetUserRoles)
                    .ThenInclude(x => x.Role);
        }

    }
}
