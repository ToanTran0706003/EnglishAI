using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class Achievement : BaseAuditableEntity
{
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? IconUrl { get; set; }
    public int XpReward { get; set; }
    public AchievementCategory Category { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string ConditionJson { get; set; } = "{}";

    public int SortOrder { get; set; }

    public List<UserAchievement> UserAchievements { get; set; } = new();
}

