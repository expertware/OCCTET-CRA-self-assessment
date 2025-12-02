using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;

namespace exp.Infrastructure.Repositories.Attachments
{
    public interface IQuestionAttachments : IGenericRepository<QuestionAttachment>
    {
        public IQueryable<QuestionAttachment> GetAttachments();
    }
}
