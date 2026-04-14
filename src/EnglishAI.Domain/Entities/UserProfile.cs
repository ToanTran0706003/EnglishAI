using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class UserProfile : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string NativeLanguage { get; set; } = null!;
    public CefrLevel CurrentLevel { get; set; } = CefrLevel.A1;
    public CefrLevel TargetLevel { get; set; } = CefrLevel.B2;
    public int DailyGoalMinutes { get; set; } = 15;
    public string? LearningPurpose { get; set; }

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string WeakSkills { get; set; } = "[]";

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string Interests { get; set; } = "[]";

    public bool OnboardingCompleted { get; set; }
    public string? Timezone { get; set; }

    public User User { get; set; } = null!;
}

