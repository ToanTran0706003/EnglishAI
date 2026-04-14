using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class LearningPathUnit : BaseAuditableEntity
{
    public Guid LearningPathId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsLocked { get; set; } = true;
    public bool IsCompleted { get; set; }
    public LessonType UnitType { get; set; }

    public LearningPath LearningPath { get; set; } = null!;
    public List<UnitLesson> UnitLessons { get; set; } = new();
}

