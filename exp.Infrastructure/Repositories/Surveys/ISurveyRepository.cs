using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.Surveys
{
    public interface ISurveyRepository : IGenericRepository<Survey>
    {
        public IQueryable<Survey> GetAll();
    }
}
