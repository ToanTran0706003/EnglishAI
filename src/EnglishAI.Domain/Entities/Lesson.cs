using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class Lesson : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public LessonType LessonType { get; set; }
    public CefrLevel CefrLevel { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string ContentJson { get; set; } = null!;

    public int DurationMin { get; set; } = 10;
    public int XpReward { get; set; } = 10;
    public int Difficulty { get; set; } = 1;

    /// <summary>PostgreSQL JSONB (array of strings).</summary>
    public string Tags { get; set; } = "[]";

    public bool IsPublished { get; set; }
    public bool IsAiGenerated { get; set; }

    public List<UnitLesson> UnitLessons { get; set; } = new();
    public List<LessonCompletion> Completions { get; set; } = new();
}

