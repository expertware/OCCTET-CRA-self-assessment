using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class Question
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int SectionId { get; set; }

    public int QuestionTypeId { get; set; }

    public bool IsDeleted { get; set; }

    public int? Position { get; set; }

    public int? ParrentAnswerId { get; set; }

    public virtual ICollection<AnswersPrompt> AnswersPrompts { get; set; } = new List<AnswersPrompt>();

    public virtual AnswersPrompt? ParrentAnswer { get; set; }

    public virtual ICollection<QuestionAttachment> QuestionAttachments { get; set; } = new List<QuestionAttachment>();

    public virtual QuestionType QuestionType { get; set; } = null!;

    public virtual SurveySection Section { get; set; } = null!;

    public virtual ICollection<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();
}
