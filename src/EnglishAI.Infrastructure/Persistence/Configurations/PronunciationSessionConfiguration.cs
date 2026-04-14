using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class PronunciationSessionConfiguration : IEntityTypeConfiguration<PronunciationSession>
{
    public void Configure(EntityTypeBuilder<PronunciationSession> builder)
    {
        builder.ToTable("pronunciation_sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.TargetPhonemes).HasMaxLength(500);

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_pron_sessions_user");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lesson)
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

