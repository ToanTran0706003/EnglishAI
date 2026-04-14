using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class ListeningExercise : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string AudioUrl { get; set; } = null!;
    public string Transcript { get; set; } = null!;
    public CefrLevel CefrLevel { get; set; }
    public string ExerciseType { get; set; } = null!;
    public int DurationSec { get; set; }
    public string? Category { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string Questions { get; set; } = "[]";

    public bool IsPublished { get; set; } = true;

    public List<DictationAttempt> DictationAttempts { get; set; } = new();
}

