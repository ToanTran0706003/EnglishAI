using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class UserExternalLogin : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string Provider { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public string? ProviderEmail { get; set; }

    public User User { get; set; } = null!;
}

