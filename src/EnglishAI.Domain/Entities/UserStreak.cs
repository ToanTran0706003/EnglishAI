using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class UserStreak : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly? LastActivityDate { get; set; }
    public int FreezeCount { get; set; }
    public int TotalDaysLearned { get; set; }

    public User User { get; set; } = null!;
}

