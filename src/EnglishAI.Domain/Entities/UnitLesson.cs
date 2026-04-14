using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class UnitLesson : BaseAuditableEntity
{
    public Guid UnitId { get; set; }
    public Guid LessonId { get; set; }
    public int SortOrder { get; set; }

    public LearningPathUnit Unit { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}

