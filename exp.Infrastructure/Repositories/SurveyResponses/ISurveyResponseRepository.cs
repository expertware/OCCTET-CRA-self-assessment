using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.SurveyResponses
{
    public interface ISurveyResponseRepository : IGenericRepository<SurveyResponse>
    {
        IQueryable<SurveyResponse> GetAll();
        IQueryable<SurveyResponse> GetDetails();
        IQueryable<SurveyResponse> GetForReports();
    }
}
