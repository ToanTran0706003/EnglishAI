using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class DailyChallengeCompletion : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ChallengeId { get; set; }
    public decimal? Score { get; set; }
    public DateTime CompletedAt { get; set; }

    public User User { get; set; } = null!;
    public DailyChallenge Challenge { get; set; } = null!;
}

