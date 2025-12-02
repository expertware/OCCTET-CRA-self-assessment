using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp.Infrastructure.Repositories.Roles
{
    public class RoleRepository : GenericRepository<AspNetRole>, IRoleRepository
    {
        public RoleRepository(SurveyDBContext context) : base(context)
        {
        }
    }
}
