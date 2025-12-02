using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.Questions
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        public IQueryable<Question> GetDetails();
        public IQueryable<Question> GetAll();
    }
}
