using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.VwOrganisationResults
{
    public class VwSectionReports : GenericRepository<VwSectionReport>, IVwSectionReports
    {
        public VwSectionReports(SurveyDBContext context) : base(context) { }
    }
}
