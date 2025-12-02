using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.ActivitySectors
{
    public class ActivitySectorRepository : GenericRepository<ActivitySector>, IActivitySectorRepository
    {
        private readonly SurveyDBContext _context;
        public ActivitySectorRepository(SurveyDBContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable<ActivitySector> GetAll()
        {
            return _context.ActivitySectors.Include(x => x.InverseActivitySectorNavigation);
        }
        public IQueryable<ActivitySector> GetForReports()
        {
            return _context.ActivitySectors.Include(x => x.OrganisationSectors)
                                                   .ThenInclude(x => x.Organisation);
        }
    }
}
