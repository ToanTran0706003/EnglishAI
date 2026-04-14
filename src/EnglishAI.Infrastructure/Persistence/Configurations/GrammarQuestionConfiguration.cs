using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class GrammarQuestionConfiguration : IEntityTypeConfiguration<GrammarQuestion>
{
    public void Configure(EntityTypeBuilder<GrammarQuestion> builder)
    {
        builder.ToTable("grammar_questions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QuestionType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.Options).HasColumnType("jsonb");
        builder.Property(x => x.Difficulty).HasDefaultValue(1);

        builder.HasOne(x => x.Topic)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.TopicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

