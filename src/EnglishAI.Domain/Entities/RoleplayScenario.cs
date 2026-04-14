using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class RoleplayScenario : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public CefrLevel CefrLevel { get; set; }
    public string SystemPrompt { get; set; } = null!;
    public string StarterMessage { get; set; } = null!;

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string Objectives { get; set; } = "[]";

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string VocabularyHints { get; set; } = "[]";

    public bool IsPublished { get; set; } = true;

    public List<ConversationSession> Sessions { get; set; } = new();
}

