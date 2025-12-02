using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class AnswersPrompt
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string Answers { get; set; } = null!;

    public string? Prompt { get; set; }

    public string? Comment { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
