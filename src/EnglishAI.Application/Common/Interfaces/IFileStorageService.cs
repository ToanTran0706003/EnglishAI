namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// File/object storage abstraction.
/// </summary>
public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

    Task<string> GetUrlAsync(string fileName);

    Task DeleteAsync(string fileName, CancellationToken ct);
}

