using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class FlashcardReview : BaseAuditableEntity
{
    public Guid FlashcardId { get; set; }
    public Guid UserId { get; set; }
    public int Quality { get; set; }
    public int TimeSpentMs { get; set; }
    public bool WasCorrect { get; set; }
    public int PreviousInterval { get; set; }
    public int NewInterval { get; set; }
    public DateTime ReviewedAt { get; set; }

    public Flashcard Flashcard { get; set; } = null!;
    public User User { get; set; } = null!;
}

