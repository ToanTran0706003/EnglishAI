using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class LeaderboardEntryConfiguration : IEntityTypeConfiguration<LeaderboardEntry>
{
    public void Configure(EntityTypeBuilder<LeaderboardEntry> builder)
    {
        builder.ToTable("leaderboard_entries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PeriodType).HasMaxLength(10).IsRequired();
        builder.Property(x => x.PeriodKey).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TotalXp).HasDefaultValue(0);

        builder.HasIndex(x => new { x.UserId, x.PeriodType, x.PeriodKey }).IsUnique();

        builder.HasIndex(x => new { x.PeriodType, x.PeriodKey, x.TotalXp })
            .HasDatabaseName("idx_leaderboard_period");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

