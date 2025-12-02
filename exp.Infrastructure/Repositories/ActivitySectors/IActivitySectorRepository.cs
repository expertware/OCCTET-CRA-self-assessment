using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.ActivitySectors
{
    public interface IActivitySectorRepository : IGenericRepository<ActivitySector>
    {
        IQueryable<ActivitySector> GetAll();
        IQueryable<ActivitySector> GetForReports();
    }
}
