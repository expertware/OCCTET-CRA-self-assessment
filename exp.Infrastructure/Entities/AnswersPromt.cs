using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class AnswersPromt
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string Answers { get; set; } = null!;

    public string? Prompt { get; set; }

    public virtual Question Question { get; set; } = null!;
}
