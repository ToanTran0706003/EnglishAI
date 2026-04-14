using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class UserStreakConfiguration : IEntityTypeConfiguration<UserStreak>
{
    public void Configure(EntityTypeBuilder<UserStreak> builder)
    {
        builder.ToTable("user_streaks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CurrentStreak).HasDefaultValue(0);
        builder.Property(x => x.LongestStreak).HasDefaultValue(0);
        builder.Property(x => x.FreezeCount).HasDefaultValue(0);
        builder.Property(x => x.TotalDaysLearned).HasDefaultValue(0);

        builder.HasIndex(x => x.UserId).IsUnique();
    }
}

