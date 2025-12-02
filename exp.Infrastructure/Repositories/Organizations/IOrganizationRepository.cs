using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.Organisations
{
    public interface IOrganizationRepository : IGenericRepository<Organisation>
    {
        IQueryable<Organisation> GetAllOrganizations();
        IQueryable<Organisation> GetDetails();
        IQueryable<Organisation> GetAll();
    }
}
