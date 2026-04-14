using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.ToTable("achievements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IconUrl).HasMaxLength(500);
        builder.Property(x => x.Category).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.ConditionJson).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.XpReward).HasDefaultValue(0);

        builder.HasIndex(x => x.Code).IsUnique();
    }
}

