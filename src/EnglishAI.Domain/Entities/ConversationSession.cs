using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class ConversationSession : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public ConversationSessionType SessionType { get; set; }
    public Guid? ScenarioId { get; set; }
    public string AiModel { get; set; } = "gpt-4o";
    public string SystemPrompt { get; set; } = null!;
    public CefrLevel CefrLevel { get; set; }

    public int MessageCount { get; set; }
    public int? DurationSec { get; set; }
    public int? GrammarErrors { get; set; }
    public int? VocabularyUsed { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string? SessionFeedback { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public User User { get; set; } = null!;
    public RoleplayScenario? Scenario { get; set; }
    public List<ConversationMessage> Messages { get; set; } = new();
}

