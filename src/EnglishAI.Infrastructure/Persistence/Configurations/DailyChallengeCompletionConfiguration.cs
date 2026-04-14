using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class DailyChallengeCompletionConfiguration : IEntityTypeConfiguration<DailyChallengeCompletion>
{
    public void Configure(EntityTypeBuilder<DailyChallengeCompletion> builder)
    {
        builder.ToTable("daily_challenge_completions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score).HasPrecision(5, 2);

        builder.HasIndex(x => new { x.UserId, x.ChallengeId }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Challenge)
            .WithMany(x => x.Completions)
            .HasForeignKey(x => x.ChallengeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

