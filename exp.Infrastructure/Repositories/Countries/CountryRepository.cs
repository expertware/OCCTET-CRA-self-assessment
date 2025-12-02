using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Countries
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly SurveyDBContext _surveyDBContext;
        public CountryRepository(SurveyDBContext context) : base(context)
        {
            _surveyDBContext = context;
        }

        public IQueryable<Country> GetAll()
        {
            return _surveyDBContext.Countries.Include(x => x.Organisations);
        }

    }
}
