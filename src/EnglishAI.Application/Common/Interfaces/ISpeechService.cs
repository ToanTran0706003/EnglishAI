namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// Speech-to-text and text-to-speech abstraction.
/// </summary>
public interface ISpeechService
{
    Task<string> TranscribeAsync(Stream audioStream, CancellationToken ct);

    Task<Stream> SynthesizeAsync(string text, string voice, CancellationToken ct);
}

