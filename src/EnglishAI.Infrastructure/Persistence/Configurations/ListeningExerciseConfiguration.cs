using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class ListeningExerciseConfiguration : IEntityTypeConfiguration<ListeningExercise>
{
    public void Configure(EntityTypeBuilder<ListeningExercise> builder)
    {
        builder.ToTable("listening_exercises");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.AudioUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5).IsRequired();
        builder.Property(x => x.ExerciseType).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(50);
        builder.Property(x => x.Questions).HasColumnType("jsonb").HasDefaultValue("[]");
        builder.Property(x => x.IsPublished).HasDefaultValue(true);
    }
}

