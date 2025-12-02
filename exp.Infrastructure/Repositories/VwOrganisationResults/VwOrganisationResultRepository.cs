using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp.Infrastructure.Repositories.VwOrganisationResults
{
    public class VwOrganisationResultRepository : GenericRepository<VwOrganisationResult> , IVwOrganisationResultRepository
    {

        public VwOrganisationResultRepository(SurveyDBContext context):base(context)
        {

        }
    }
}
