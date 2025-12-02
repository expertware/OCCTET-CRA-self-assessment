using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.VwOrganisationResults
{
    public class OrganisationReportsRepository : GenericRepository<VwOrgannisationsReport>, IOrganisationReportsRepository
    {
        public OrganisationReportsRepository(SurveyDBContext context) : base(context)
        { }
    }
}
