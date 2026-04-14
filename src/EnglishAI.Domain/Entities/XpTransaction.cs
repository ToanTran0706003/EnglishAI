using EnglishAI.Domain.Common;
using EnglishAI.Domain.Enums;

namespace EnglishAI.Domain.Entities;

public class XpTransaction : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public int Amount { get; set; }
    public XpSource Source { get; set; }
    public Guid? SourceId { get; set; }
    public string? Description { get; set; }
    public int BalanceAfter { get; set; }

    public User User { get; set; } = null!;
}

