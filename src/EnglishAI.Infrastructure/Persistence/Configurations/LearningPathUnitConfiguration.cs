using EnglishAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishAI.Infrastructure.Persistence.Configurations;

public sealed class LearningPathUnitConfiguration : IEntityTypeConfiguration<LearningPathUnit>
{
    public void Configure(EntityTypeBuilder<LearningPathUnit> builder)
    {
        builder.ToTable("learning_path_units");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.UnitType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(x => x.IsLocked).HasDefaultValue(true);
        builder.Property(x => x.IsCompleted).HasDefaultValue(false);

        builder.HasIndex(x => x.LearningPathId).HasDatabaseName("idx_units_path");

        builder.HasOne(x => x.LearningPath)
            .WithMany(x => x.Units)
            .HasForeignKey(x => x.LearningPathId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

