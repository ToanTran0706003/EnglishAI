namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// Pronunciation scoring / assessment abstraction.
/// </summary>
public interface IPronunciationAssessmentService
{
    Task<string> EvaluateAsync(Stream audioStream, string referenceText, CancellationToken ct);
}

