using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class RoleplayScenarioConfiguration : IEntityTypeConfiguration<RoleplayScenario>
{
    public void Configure(EntityTypeBuilder<RoleplayScenario> builder)
    {
        builder.ToTable("roleplay_scenarios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CefrLevel).HasConversion<string>().HasMaxLength(5).IsRequired();
        builder.Property(x => x.Objectives).HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.VocabularyHints).HasColumnType("jsonb").HasDefaultValue("[]");
        builder.Property(x => x.IsPublished).HasDefaultValue(true);

        builder.HasIndex(x => x.CefrLevel).HasDatabaseName("idx_scenarios_level");
    }
}

