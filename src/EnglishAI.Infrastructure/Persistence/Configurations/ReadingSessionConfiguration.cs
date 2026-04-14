using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class ReadingSessionConfiguration : IEntityTypeConfiguration<ReadingSession>
{
    public void Configure(EntityTypeBuilder<ReadingSession> builder)
    {
        builder.ToTable("reading_sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TimeSpentSec).HasDefaultValue(0);
        builder.Property(x => x.ProgressPct).HasPrecision(5, 2).HasDefaultValue(0m);
        builder.Property(x => x.QuizScore).HasPrecision(5, 2);
        builder.Property(x => x.WordsLookedUp).HasColumnType("jsonb").HasDefaultValue("[]");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Article)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.ArticleId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

