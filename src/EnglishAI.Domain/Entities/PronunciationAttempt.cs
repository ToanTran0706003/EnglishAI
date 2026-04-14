using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class PronunciationAttempt : BaseAuditableEntity
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }

    public string AudioUrl { get; set; } = null!;
    public decimal AudioDurationSec { get; set; }

    public string? RecognizedText { get; set; }
    public string? RecognizedPhonemes { get; set; }

    public decimal? AccuracyScore { get; set; }
    public decimal? FluencyScore { get; set; }
    public decimal? CompletenessScore { get; set; }
    public decimal? PronunciationScore { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string? PhonemeScores { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string? WordScores { get; set; }

    public string? FeedbackText { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string? WaveformData { get; set; }

    public DateTime AttemptedAt { get; set; }

    public PronunciationSession Session { get; set; } = null!;
    public User User { get; set; } = null!;
}

