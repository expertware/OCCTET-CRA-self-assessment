using exp.Infrastructure.Context;
using exp.Infrastructure.Entities;
using exp.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace exp.Infrastructure.Repositories.Attachments
{
    public class QuestionAttachments : GenericRepository<QuestionAttachment>, IQuestionAttachments
    {
        private readonly SurveyDBContext _surveyDBContext;
        public QuestionAttachments(SurveyDBContext context) : base(context)
        {
            _surveyDBContext = context;
        }

        public IQueryable<QuestionAttachment> GetAttachments()
        {
            return _surveyDBContext.QuestionAttachments
                .Include(x => x.Attachment)
                .Include(x => x.Question)
                    .ThenInclude(x => x.Section);
        }
    }
}
