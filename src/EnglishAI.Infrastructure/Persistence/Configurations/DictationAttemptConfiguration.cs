using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class DictationAttemptConfiguration : IEntityTypeConfiguration<DictationAttempt>
{
    public void Configure(EntityTypeBuilder<DictationAttempt> builder)
    {
        builder.ToTable("dictation_attempts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccuracyPct).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.Errors).HasColumnType("jsonb").IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.DictationAttempts)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

