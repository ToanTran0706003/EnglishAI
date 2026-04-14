using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class ReadingArticleConfiguration : IEntityTypeConfiguration<ReadingArticle>
{
    public void Configure(EntityTypeBuilder<ReadingArticle> builder)
    {
        builder.ToTable("reading_articles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(50).IsRequired();
        builder.Property(x => x.SourceUrl).HasMaxLength(500);
        builder.Property(x => x.ImageUrl).HasMaxLength(500);
        builder.Property(x => x.IsPublished).HasDefaultValue(true);
        builder.Property(x => x.IsAiGenerated).HasDefaultValue(false);

        builder.Property(x => x.VocabularyList).HasColumnType("jsonb").HasDefaultValue("[]");
        builder.Property(x => x.ComprehensionQuestions).HasColumnType("jsonb").HasDefaultValue("[]");

        builder.HasIndex(x => x.CefrLevel).HasDatabaseName("idx_articles_level");
    }
}

