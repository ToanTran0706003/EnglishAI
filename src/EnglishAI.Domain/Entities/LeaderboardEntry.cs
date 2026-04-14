using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class LeaderboardEntry : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string PeriodType { get; set; } = null!;
    public string PeriodKey { get; set; } = null!;
    public int TotalXp { get; set; }
    public int? Rank { get; set; }

    public User User { get; set; } = null!;
}

