using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class ReadingArticle : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Summary { get; set; }
    public CefrLevel CefrLevel { get; set; }
    public string Category { get; set; } = null!;
    public int WordCount { get; set; }
    public int EstimatedMin { get; set; }

    /// <summary>PostgreSQL JSONB.</summary>
    public string VocabularyList { get; set; } = "[]";

    /// <summary>PostgreSQL JSONB.</summary>
    public string ComprehensionQuestions { get; set; } = "[]";

    public string? SourceUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAiGenerated { get; set; }
    public bool IsPublished { get; set; } = true;

    public List<ReadingSession> Sessions { get; set; } = new();
}

