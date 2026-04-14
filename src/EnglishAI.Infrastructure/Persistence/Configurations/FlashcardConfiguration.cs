using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class FlashcardConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.ToTable("flashcards");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Front).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Back).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Phonetic).HasMaxLength(200);
        builder.Property(x => x.AudioUrl).HasMaxLength(500);
        builder.Property(x => x.ImageUrl).HasMaxLength(500);
        builder.Property(x => x.PartOfSpeech).HasMaxLength(20);

        builder.Property(x => x.EaseFactor).HasPrecision(4, 2).HasDefaultValue(2.50m);
        builder.Property(x => x.IntervalDays).HasDefaultValue(0);
        builder.Property(x => x.Repetitions).HasDefaultValue(0);
        builder.Property(x => x.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => x.DeckId)
            .HasDatabaseName("idx_flashcards_deck")
            .HasFilter("NOT is_deleted");

        builder.HasIndex(x => new { x.DeckId, x.NextReviewAt })
            .HasDatabaseName("idx_flashcards_due")
            .HasFilter("NOT is_deleted");

        builder.HasOne(x => x.Deck)
            .WithMany(x => x.Cards)
            .HasForeignKey(x => x.DeckId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

