using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class PronunciationSession : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public PronunciationSessionType SessionType { get; set; }
    public string TargetText { get; set; } = null!;
    public string? TargetPhonemes { get; set; }
    public CefrLevel? CefrLevel { get; set; }
    public Guid? LessonId { get; set; }

    public User User { get; set; } = null!;
    public Lesson? Lesson { get; set; }
    public List<PronunciationAttempt> Attempts { get; set; } = new();
}

