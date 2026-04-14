using EnglishAI.Domain.Entities;
using EnglishAI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NativeLanguage).HasMaxLength(10).IsRequired().HasDefaultValue("vi");
        builder.Property(x => x.CurrentLevel).HasConversion<string>().HasMaxLength(5).IsRequired().HasDefaultValue(CefrLevel.A1);
        builder.Property(x => x.TargetLevel).HasConversion<string>().HasMaxLength(5).IsRequired().HasDefaultValue(CefrLevel.B2);
        builder.Property(x => x.DailyGoalMinutes).HasDefaultValue(15);
        builder.Property(x => x.LearningPurpose).HasMaxLength(50);

        builder.Property(x => x.WeakSkills).HasColumnType("jsonb").HasDefaultValue("[]");
        builder.Property(x => x.Interests).HasColumnType("jsonb").HasDefaultValue("[]");

        builder.Property(x => x.Timezone).HasMaxLength(50).HasDefaultValue("Asia/Ho_Chi_Minh");

        builder.HasIndex(x => x.UserId).IsUnique();
    }
}

