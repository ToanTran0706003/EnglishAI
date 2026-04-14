using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class XpTransactionConfiguration : IEntityTypeConfiguration<XpTransaction>
{
    public void Configure(EntityTypeBuilder<XpTransaction> builder)
    {
        builder.ToTable("xp_transactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Source).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);

        builder.HasIndex(x => x.UserId).HasDatabaseName("idx_xp_user");
        builder.HasIndex(x => new { x.UserId, x.CreatedAt }).HasDatabaseName("idx_xp_user_date");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

