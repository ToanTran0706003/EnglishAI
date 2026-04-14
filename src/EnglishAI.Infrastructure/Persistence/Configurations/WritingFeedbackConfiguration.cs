using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class WritingFeedbackConfiguration : IEntityTypeConfiguration<WritingFeedback>
{
    public void Configure(EntityTypeBuilder<WritingFeedback> builder)
    {
        builder.ToTable("writing_feedback");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OverallScore).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.GrammarScore).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.VocabularyScore).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.CoherenceScore).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.TaskAchievement).HasPrecision(5, 2).IsRequired();

        builder.Property(x => x.InlineCorrections).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.ImprovementTips).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.VocabularySuggestions).HasColumnType("jsonb").HasDefaultValue("[]");

        builder.Property(x => x.AiModel).HasMaxLength(50).IsRequired();
    }
}

