using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class DailyChallenge : BaseAuditableEntity
{
    public DateOnly ChallengeDate { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ChallengeType { get; set; } = null!;

    /// <summary>PostgreSQL JSONB.</summary>
    public string ContentJson { get; set; } = null!;

    public int XpReward { get; set; } = 20;

    public List<DailyChallengeCompletion> Completions { get; set; } = new();
}

