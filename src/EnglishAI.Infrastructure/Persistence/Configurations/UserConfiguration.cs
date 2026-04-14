using EnglishAI.Domain.Entities;
using EnglishAI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
        builder.Property(x => x.NormalizedEmail).HasMaxLength(255).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(500);
        builder.Property(x => x.DisplayName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.AvatarUrl).HasMaxLength(500);
        builder.Property(x => x.Role).HasConversion<string>().HasMaxLength(20).HasDefaultValue(UserRole.Learner);

        builder.HasIndex(x => x.NormalizedEmail).HasDatabaseName("idx_users_email");

        builder.HasOne(x => x.Profile)
            .WithOne(x => x.User)
            .HasForeignKey<UserProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Settings)
            .WithOne(x => x.User)
            .HasForeignKey<UserSettings>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Streak)
            .WithOne(x => x.User)
            .HasForeignKey<UserStreak>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

