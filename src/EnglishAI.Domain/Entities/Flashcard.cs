using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class Flashcard : BaseAuditableEntity, ISoftDeletable
{
    public Guid DeckId { get; set; }
    public string Front { get; set; } = null!;
    public string Back { get; set; } = null!;
    public string? Phonetic { get; set; }
    public string? ExampleSentence { get; set; }
    public string? AudioUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? PartOfSpeech { get; set; }

    public decimal EaseFactor { get; set; } = 2.50m;
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public DateTime NextReviewAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReviewedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public FlashcardDeck Deck { get; set; } = null!;
    public List<FlashcardReview> Reviews { get; set; } = new();
}

