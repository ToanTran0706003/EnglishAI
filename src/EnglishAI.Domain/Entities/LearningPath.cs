using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class LearningPath : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public CefrLevel CefrLevel { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal ProgressPct { get; set; }

    public User User { get; set; } = null!;
    public List<LearningPathUnit> Units { get; set; } = new();
}

