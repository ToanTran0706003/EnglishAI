using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class UnitLessonConfiguration : IEntityTypeConfiguration<UnitLesson>
{
    public void Configure(EntityTypeBuilder<UnitLesson> builder)
    {
        builder.ToTable("unit_lessons");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.UnitId, x.LessonId }).IsUnique();

        builder.HasOne(x => x.Unit)
            .WithMany(x => x.UnitLessons)
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Lesson)
            .WithMany(x => x.UnitLessons)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

