using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class VocabularyBank : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string Word { get; set; } = null!;
    public string Definition { get; set; } = null!;
    public string? Phonetic { get; set; }
    public string? PartOfSpeech { get; set; }
    public string? ExampleSentence { get; set; }
    public string? AudioUrl { get; set; }
    public string? Source { get; set; }
    public Guid? SourceId { get; set; }
    public int MasteryLevel { get; set; }
    public bool IsFavorite { get; set; }

    public User User { get; set; } = null!;
}

