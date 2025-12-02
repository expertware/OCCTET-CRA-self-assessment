using System;
using System.Collections.Generic;

namespace exp.Infrastructure.Entities;

public partial class SurveySection
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int SurveyId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ParentSectionId { get; set; }

    public virtual ICollection<SurveySection> InverseParentSection { get; set; } = new List<SurveySection>();

    public virtual SurveySection? ParentSection { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual Survey Survey { get; set; } = null!;
}
