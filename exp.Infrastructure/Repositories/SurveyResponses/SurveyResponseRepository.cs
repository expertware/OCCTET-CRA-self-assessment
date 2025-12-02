using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.SurveyResponses
{
    public class SurveyResponseRepository : GenericRepository<SurveyResponse>, ISurveyResponseRepository
    {
        private readonly SurveyDBContext _context;
        public SurveyResponseRepository(SurveyDBContext context) : base(context)
        {

            _context = context;
        }

        public IQueryable<SurveyResponse> GetAll()
        {
            return _context.SurveyResponses.Include(x => x.Question)
                                                .ThenInclude(x => x.ParrentAnswer);
        }
        public IQueryable<SurveyResponse> GetDetails()
        {
            return _context.SurveyResponses
                .Include(x => x.OrganisationSurvey)
                .Include(x => x.Question)
                    .ThenInclude(x => x.Section)
                .Include(x => x.Question)
                    .ThenInclude(x => x.ParrentAnswer);
        }
        public IQueryable<SurveyResponse> GetForReports()
        {
            return _context.SurveyResponses
                .Include(x => x.OrganisationSurvey);
        }
    }
}
