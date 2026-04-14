using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class WritingSubmission : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string PromptText { get; set; } = null!;
    public WritingPromptType PromptType { get; set; }
    public CefrLevel CefrLevel { get; set; }
    public string UserText { get; set; } = null!;
    public int WordCount { get; set; }
    public Guid? LessonId { get; set; }
    public DateTime SubmittedAt { get; set; }

    public User User { get; set; } = null!;
    public Lesson? Lesson { get; set; }
    public WritingFeedback? Feedback { get; set; }
}

