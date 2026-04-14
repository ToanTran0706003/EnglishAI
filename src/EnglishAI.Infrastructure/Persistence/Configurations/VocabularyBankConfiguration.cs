using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class VocabularyBankConfiguration : IEntityTypeConfiguration<VocabularyBank>
{
    public void Configure(EntityTypeBuilder<VocabularyBank> builder)
    {
        builder.ToTable("vocabulary_bank");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Word).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phonetic).HasMaxLength(200);
        builder.Property(x => x.PartOfSpeech).HasMaxLength(20);
        builder.Property(x => x.AudioUrl).HasMaxLength(500);
        builder.Property(x => x.Source).HasMaxLength(50);
        builder.Property(x => x.MasteryLevel).HasDefaultValue(0);
        builder.Property(x => x.IsFavorite).HasDefaultValue(false);

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_vocab_user");
        builder.HasIndex(x => new { x.UserId, x.Word }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

