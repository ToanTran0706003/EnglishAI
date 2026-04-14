using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class GrammarQuizAttemptConfiguration : IEntityTypeConfiguration<GrammarQuizAttempt>
{
    public void Configure(EntityTypeBuilder<GrammarQuizAttempt> builder)
    {
        builder.ToTable("grammar_quiz_attempts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Score).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.Answers).HasColumnType("jsonb").IsRequired();

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_grammar_attempts_user");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Topic)
            .WithMany(x => x.QuizAttempts)
            .HasForeignKey(x => x.TopicId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

