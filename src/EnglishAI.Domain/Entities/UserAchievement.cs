using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class UserAchievement : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid AchievementId { get; set; }
    public DateTime UnlockedAt { get; set; }

    public User User { get; set; } = null!;
    public Achievement Achievement { get; set; } = null!;
}

