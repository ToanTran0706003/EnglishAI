namespace EnglishAI.Application.Common.Interfaces;

/// <summary>
/// Provides access to the currently authenticated user (if any).
/// </summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}

