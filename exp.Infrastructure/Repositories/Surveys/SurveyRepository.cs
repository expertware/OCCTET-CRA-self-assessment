using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Surveys
{
    public class SurveyRepository : GenericRepository<Survey>, ISurveyRepository
    {
        private readonly SurveyDBContext _context;
        public SurveyRepository(SurveyDBContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable<Survey> GetAll()
        {
            return _context.Surveys
                .Include(s => s.SurveySections)
                         .ThenInclude(x => x.InverseParentSection)
                            .ThenInclude(sec => sec.Questions)
                                  .ThenInclude(q => q.ParrentAnswer)
                 .Include(s => s.SurveySections)
                         .ThenInclude(x => x.InverseParentSection)
                            .ThenInclude(sec => sec.Questions)
                                  .ThenInclude(q => q.AnswersPrompts)
                  .Include(s => s.SurveySections)
                         .ThenInclude(x => x.InverseParentSection)
                            .ThenInclude(sec => sec.Questions)
                                .ThenInclude(x => x.QuestionType)
                 .Include(s => s.SurveySections)
                      .ThenInclude(sec => sec.Questions)
                          .ThenInclude(q => q.AnswersPrompts)
                .Include(s => s.SurveySections)
                      .ThenInclude(sec => sec.Questions)
                          .ThenInclude(q => q.ParrentAnswer)
                  .Include(s => s.SurveySections)
                      .ThenInclude(sec => sec.Questions)
                          .ThenInclude(q => q.QuestionType)
                  .Include(s => s.SurveySections)
                      .ThenInclude(sec => sec.Questions)
                          .ThenInclude(q => q.QuestionAttachments)
                              .ThenInclude(qa => qa.Attachment);

        }
    }
}
