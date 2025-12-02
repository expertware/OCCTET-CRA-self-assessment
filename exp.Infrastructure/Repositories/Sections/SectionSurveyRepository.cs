using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Sections
{
    public class SectionSurveyRepository : GenericRepository<SurveySection>, ISectionSurveyRepository
    {
        private readonly SurveyDBContext _surveyDBContext;
        public SectionSurveyRepository(SurveyDBContext context) : base(context) 
        {
            _surveyDBContext = context; 
        }

        public IQueryable<SurveySection> GetAll()
        {
            return _surveyDBContext.SurveySections
                .Include(x => x.InverseParentSection)   
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.ParrentAnswer)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.ParrentAnswer)
                .Include(x => x.Survey);
        }
    }
}
