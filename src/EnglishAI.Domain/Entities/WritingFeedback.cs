using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class WritingFeedback : BaseAuditableEntity
{
    public Guid SubmissionId { get; set; }
    public decimal OverallScore { get; set; }
    public decimal GrammarScore { get; set; }
    public decimal VocabularyScore { get; set; }
    public decimal CoherenceScore { get; set; }
    public decimal TaskAchievement { get; set; }

    public string CorrectedText { get; set; } = null!;

    /// <summary>PostgreSQL JSONB.</summary>
    public string InlineCorrections { get; set; } = "[]";

    public string GeneralFeedback { get; set; } = null!;

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string ImprovementTips { get; set; } = "[]";

    /// <summary>PostgreSQL JSONB.</summary>
    public string VocabularySuggestions { get; set; } = "[]";

    public string AiModel { get; set; } = null!;

    public WritingSubmission Submission { get; set; } = null!;
}

