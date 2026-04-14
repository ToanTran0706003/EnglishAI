using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class ConversationMessage : BaseAuditableEntity
{
    public Guid SessionId { get; set; }
    public string Role { get; set; } = null!;
    public string Content { get; set; } = null!;

    /// <summary>PostgreSQL JSONB.</summary>
    public string? GrammarIssues { get; set; }

    public string? VocabLevel { get; set; }
    public string? AudioUrl { get; set; }
    public int? TokenCount { get; set; }
    public int SortOrder { get; set; }

    public ConversationSession Session { get; set; } = null!;
}

