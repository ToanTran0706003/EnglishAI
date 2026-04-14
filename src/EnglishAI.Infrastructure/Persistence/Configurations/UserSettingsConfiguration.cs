using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.ToTable("user_settings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LanguageUi).HasMaxLength(10).IsRequired().HasDefaultValue("vi");
        builder.Property(x => x.NotificationEnabled).HasDefaultValue(true);
        builder.Property(x => x.SoundEnabled).HasDefaultValue(true);
        builder.Property(x => x.DarkMode).HasDefaultValue(false);
        builder.Property(x => x.AutoPlayAudio).HasDefaultValue(true);
        builder.Property(x => x.ShowPhonetic).HasDefaultValue(true);

        builder.HasIndex(x => x.UserId).IsUnique();
    }
}

