using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class GrammarQuestion : BaseAuditableEntity
{
    public Guid TopicId { get; set; }
    public QuestionType QuestionType { get; set; }
    public string QuestionText { get; set; } = null!;

    /// <summary>PostgreSQL JSONB.</summary>
    public string? Options { get; set; }

    public string CorrectAnswer { get; set; } = null!;
    public string Explanation { get; set; } = null!;
    public int Difficulty { get; set; } = 1;

    public GrammarTopic Topic { get; set; } = null!;
}

