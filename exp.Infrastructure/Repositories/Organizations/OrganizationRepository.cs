using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Organisations
{
    public class OrganizationRepository : GenericRepository<Organisation>, IOrganizationRepository
    {
        private readonly SurveyDBContext _context;
        public OrganizationRepository(SurveyDBContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Organisation> GetAllOrganizations()
        {
            return _context.Organisations
                .Include(x => x.Country)
                .Include(x => x.OrganisationSectors)
                    .ThenInclude(x => x.ActivitySector)
                .Include(x => x.DeliveryCountries)
                    .ThenInclude(x => x.Country);
        }
        public IQueryable<Organisation> GetAll()
        {
            return _context.Organisations
                .Include(x => x.Country)
                .Include(x => x.OrganisationSectors)
                    .ThenInclude(x => x.ActivitySector)
                .Include(x => x.DeliveryCountries)
                    .ThenInclude(x => x.Country)
                 .Include(x => x.OrganisationSurveys);
        }
        public IQueryable<Organisation> GetDetails()
        {
            return _context.Organisations
                .Include(x => x.Account);
        }
        
    }
}
