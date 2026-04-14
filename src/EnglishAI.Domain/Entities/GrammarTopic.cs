using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class GrammarTopic : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Explanation { get; set; } = null!;

    /// <summary>PostgreSQL JSONB.</summary>
    public string Examples { get; set; } = "[]";

    public CefrLevel CefrLevel { get; set; }
    public string Category { get; set; } = null!;
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; } = true;

    public List<GrammarQuestion> Questions { get; set; } = new();
    public List<GrammarQuizAttempt> QuizAttempts { get; set; } = new();
}

