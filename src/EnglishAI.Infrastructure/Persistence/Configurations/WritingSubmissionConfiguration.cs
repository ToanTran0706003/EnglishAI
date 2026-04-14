using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class WritingSubmissionConfiguration : IEntityTypeConfiguration<WritingSubmission>
{
    public void Configure(EntityTypeBuilder<WritingSubmission> builder)
    {
        builder.ToTable("writing_submissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PromptType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5).IsRequired();

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_writing_user");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lesson)
            .WithMany()
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Feedback)
            .WithOne(x => x.Submission)
            .HasForeignKey<WritingFeedback>(x => x.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

