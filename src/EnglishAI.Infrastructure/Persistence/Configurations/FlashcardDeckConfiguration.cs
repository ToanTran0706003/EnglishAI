using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class FlashcardDeckConfiguration : IEntityTypeConfiguration<FlashcardDeck>
{
    public void Configure(EntityTypeBuilder<FlashcardDeck> builder)
    {
        builder.ToTable("flashcard_decks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Tags).HasColumnType("jsonb").HasDefaultValue("[]");
        builder.Property(x => x.IsPublic).HasDefaultValue(false);
        builder.Property(x => x.CardCount).HasDefaultValue(0);
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5);

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_decks_user")
            .HasFilter("NOT is_deleted");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

