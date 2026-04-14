namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// Abstraction for LLM text completion (standard + streaming).
/// </summary>
public interface IAiCompletionService
{
    Task<string> GetCompletionAsync(
        string systemPrompt,
        string userMessage,
        int maxTokens,
        double temperature,
        CancellationToken ct);

    IAsyncEnumerable<string> StreamCompletionAsync(
        string systemPrompt,
        string userMessage,
        int maxTokens,
        double temperature,
        CancellationToken ct);
}

