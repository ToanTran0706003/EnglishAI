using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class GrammarQuizAttempt : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public decimal Score { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectCount { get; set; }
    public int TimeSpentSec { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string Answers { get; set; } = "[]";

    public DateTime AttemptedAt { get; set; }

    public User User { get; set; } = null!;
    public GrammarTopic Topic { get; set; } = null!;
}

