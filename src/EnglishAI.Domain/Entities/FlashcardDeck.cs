using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class FlashcardDeck : BaseAuditableEntity, ISoftDeletable
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public int CardCount { get; set; }
    public CefrLevel? CefrLevel { get; set; }

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string Tags { get; set; } = "[]";

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User User { get; set; } = null!;
    public List<Flashcard> Cards { get; set; } = new();
}

