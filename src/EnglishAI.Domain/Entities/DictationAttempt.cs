using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class DictationAttempt : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ExerciseId { get; set; }
    public string UserText { get; set; } = null!;
    public decimal AccuracyPct { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string Errors { get; set; } = "[]";

    public int TimeSpentSec { get; set; }
    public DateTime AttemptedAt { get; set; }

    public User User { get; set; } = null!;
    public ListeningExercise Exercise { get; set; } = null!;
}

