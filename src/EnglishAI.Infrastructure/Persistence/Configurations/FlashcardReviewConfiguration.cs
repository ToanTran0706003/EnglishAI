using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class FlashcardReviewConfiguration : IEntityTypeConfiguration<FlashcardReview>
{
    public void Configure(EntityTypeBuilder<FlashcardReview> builder)
    {
        builder.ToTable("flashcard_reviews");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quality).IsRequired();
        builder.Property(x => x.TimeSpentMs).IsRequired();
        builder.Property(x => x.WasCorrect).IsRequired();
        builder.Property(x => x.PreviousInterval).IsRequired();
        builder.Property(x => x.NewInterval).IsRequired();

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_reviews_user");
        builder.HasIndex(x => x.FlashcardId).HasDatabaseName("idx_reviews_card");

        builder.HasOne(x => x.Flashcard)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.FlashcardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

