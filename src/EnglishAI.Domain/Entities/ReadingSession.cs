using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class ReadingSession : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
    public int TimeSpentSec { get; set; }
    public decimal ProgressPct { get; set; }
    public decimal? QuizScore { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string WordsLookedUp { get; set; } = "[]";

    public DateTime? CompletedAt { get; set; }

    public User User { get; set; } = null!;
    public ReadingArticle Article { get; set; } = null!;
}

