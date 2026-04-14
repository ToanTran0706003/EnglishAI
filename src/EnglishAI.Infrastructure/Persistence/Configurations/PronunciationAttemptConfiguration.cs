using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class PronunciationAttemptConfiguration : IEntityTypeConfiguration<PronunciationAttempt>
{
    public void Configure(EntityTypeBuilder<PronunciationAttempt> builder)
    {
        builder.ToTable("pronunciation_attempts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AudioUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.AudioDurationSec).HasPrecision(6, 2).IsRequired();
        builder.Property(x => x.RecognizedPhonemes).HasMaxLength(500);

        builder.Property(x => x.AccuracyScore).HasPrecision(5, 2);
        builder.Property(x => x.FluencyScore).HasPrecision(5, 2);
        builder.Property(x => x.CompletenessScore).HasPrecision(5, 2);
        builder.Property(x => x.PronunciationScore).HasPrecision(5, 2);

        builder.Property(x => x.PhonemeScores).HasColumnType("jsonb");
        builder.Property(x => x.WordScores).HasColumnType("jsonb");
        builder.Property(x => x.WaveformData).HasColumnType("jsonb");

        builder.HasIndex(x => x.SessionId).HasDatabaseName("idx_pron_attempts_session");
        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_pron_attempts_user");

        builder.HasOne(x => x.Session)
            .WithMany(x => x.Attempts)
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

