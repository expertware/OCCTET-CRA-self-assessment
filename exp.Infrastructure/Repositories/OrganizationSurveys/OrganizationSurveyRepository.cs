using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.OrganisationSurveys
{
    public class OrganizationSurveyRepository : GenericRepository<OrganisationSurvey>, IOrganizationSurveyRepository
    {
        private readonly SurveyDBContext _context;
        public OrganizationSurveyRepository(SurveyDBContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<OrganisationSurvey> GetAll()
        {
            return _context.OrganisationSurveys
                .Include(x => x.SurveyResponses)
                .Include(x => x.Survey)
                    .ThenInclude(x => x.SurveySections)
                        .ThenInclude(x => x.Questions)
                .Include(x => x.Survey)
                    .AsNoTracking().AsQueryable();
        }
        public IQueryable<OrganisationSurvey> GetForRaport()
        {
            return _context.OrganisationSurveys
            .Include(x => x.Organisation)
                .ThenInclude(x => x.OrganisationSectors);
        }
        public IQueryable<OrganisationSurvey> GetDetails()
        {
            return _context.OrganisationSurveys
            .Include(x => x.Survey);
        }
    }
}
