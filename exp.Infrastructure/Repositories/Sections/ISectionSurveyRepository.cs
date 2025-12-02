using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exp.Infrastructure.Repositories.Sections
{
    public interface ISectionSurveyRepository : IGenericRepository<SurveySection>
    {
        public IQueryable<SurveySection> GetAll();
    }
}
