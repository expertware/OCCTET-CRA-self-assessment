using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class QuestionAttachment
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int AttachmentId { get; set; }

    public virtual Attachment Attachment { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
