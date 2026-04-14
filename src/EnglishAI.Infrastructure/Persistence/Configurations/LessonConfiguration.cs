using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.ToTable("lessons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.LessonType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5).IsRequired();
        builder.Property(x => x.ContentJson).HasColumnType("jsonb").IsRequired();

        builder.Property(x => x.DurationMin).HasDefaultValue(10);
        builder.Property(x => x.XpReward).HasDefaultValue(10);
        builder.Property(x => x.Difficulty).HasDefaultValue(1);

        builder.Property(x => x.Tags).HasColumnType("jsonb").HasDefaultValue("[]");
        builder.Property(x => x.IsPublished).HasDefaultValue(false);
        builder.Property(x => x.IsAiGenerated).HasDefaultValue(false);

        builder.HasIndex(x => new { x.LessonType, x.CefrLevel }).HasDatabaseName("idx_lessons_type_level");
    }
}

