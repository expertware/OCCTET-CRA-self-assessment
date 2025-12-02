using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.Countries
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        public IQueryable<Country> GetAll();
    }
}
