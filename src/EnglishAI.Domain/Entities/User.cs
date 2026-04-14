using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public string DisplayName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public UserRole Role { get; set; } = UserRole.Learner;
    public bool EmailConfirmed { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    public UserProfile? Profile { get; set; }
    public UserSettings? Settings { get; set; }
    public UserStreak? Streak { get; set; }

    public List<UserExternalLogin> ExternalLogins { get; set; } = new();
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}

