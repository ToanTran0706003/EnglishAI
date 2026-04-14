using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class ConversationSessionConfiguration : IEntityTypeConfiguration<ConversationSession>
{
    public void Configure(EntityTypeBuilder<ConversationSession> builder)
    {
        builder.ToTable("conversation_sessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.AiModel).HasMaxLength(50).IsRequired().HasDefaultValue("gpt-4o");
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5).IsRequired();
        builder.Property(x => x.SessionFeedback).HasColumnType("jsonb");

        builder.Property(x => x.MessageCount).HasDefaultValue(0);
        builder.Property(x => x.GrammarErrors).HasDefaultValue(0);
        builder.Property(x => x.VocabularyUsed).HasDefaultValue(0);

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_conv_sessions_user");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Scenario)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.ScenarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

