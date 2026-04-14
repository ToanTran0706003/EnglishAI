using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class RefreshToken : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid? ReplacedBy { get; set; }
    public string? CreatedByIp { get; set; }

    public User User { get; set; } = null!;
}

