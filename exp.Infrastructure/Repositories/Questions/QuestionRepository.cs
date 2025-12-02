using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Questions
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly SurveyDBContext _context;
        public QuestionRepository(SurveyDBContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Question> GetDetails()
        {
            return _context.Questions
                            .Include(x => x.QuestionType)
                            .Include(x => x.Section)
                            .Include(x => x.AnswersPrompts)
                                .ThenInclude(x => x.Questions)
                                    .ThenInclude(x => x.AnswersPrompts)
                             .Include(x => x.AnswersPrompts)
                                    .ThenInclude(x => x.Questions)
                                        .ThenInclude(x => x.QuestionType)
                             .Include(x => x.AnswersPrompts)
                                .ThenInclude(x => x.Questions)
                                    .ThenInclude(x => x.Section);
        }
        public IQueryable<Question> GetAll()
        {
            return _context.Questions
                            .Include(x => x.Section)
                            .Include(x => x.AnswersPrompts);
        }
    }
}
