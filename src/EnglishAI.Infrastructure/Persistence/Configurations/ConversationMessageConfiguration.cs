using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class ConversationMessageConfiguration : IEntityTypeConfiguration<ConversationMessage>
{
    public void Configure(EntityTypeBuilder<ConversationMessage> builder)
    {
        builder.ToTable("conversation_messages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role).HasMaxLength(15).IsRequired();
        builder.Property(x => x.GrammarIssues).HasColumnType("jsonb");
        builder.Property(x => x.VocabLevel).HasMaxLength(5);
        builder.Property(x => x.AudioUrl).HasMaxLength(500);

        builder.HasIndex(x => x.SessionId).HasDatabaseName("idx_conv_messages_session");

        builder.HasOne(x => x.Session)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

