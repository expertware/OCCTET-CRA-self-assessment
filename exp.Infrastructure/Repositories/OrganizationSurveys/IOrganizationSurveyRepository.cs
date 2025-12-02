using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.OrganisationSurveys
{
    public interface IOrganizationSurveyRepository : IGenericRepository<OrganisationSurvey>
    {
        public IQueryable<OrganisationSurvey> GetAll();
        public IQueryable<OrganisationSurvey> GetForRaport();
        public IQueryable<OrganisationSurvey> GetDetails();
    }
}
