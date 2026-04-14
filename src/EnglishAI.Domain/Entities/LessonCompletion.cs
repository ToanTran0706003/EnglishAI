using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class LessonCompletion : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid LessonId { get; set; }
    public decimal? Score { get; set; }
    public int TimeSpentSec { get; set; }
    public int XpEarned { get; set; }
    public DateTime CompletedAt { get; set; }
    public int AttemptNumber { get; set; } = 1;

    public User User { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}

