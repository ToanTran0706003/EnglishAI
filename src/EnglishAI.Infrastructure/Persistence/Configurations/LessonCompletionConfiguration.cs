using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class LessonCompletionConfiguration : IEntityTypeConfiguration<LessonCompletion>
{
    public void Configure(EntityTypeBuilder<LessonCompletion> builder)
    {
        builder.ToTable("lesson_completions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score).HasPrecision(5, 2);
        builder.Property(x => x.XpEarned).HasDefaultValue(0);
        builder.Property(x => x.AttemptNumber).HasDefaultValue(1);

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_completions_user");
        builder.HasIndex(x => new { x.UserId, x.LessonId }).HasDatabaseName("idx_completions_lesson");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lesson)
            .WithMany(x => x.Completions)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

