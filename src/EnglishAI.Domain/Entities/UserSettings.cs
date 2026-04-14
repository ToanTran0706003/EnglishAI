using EnglishAI.Domain.Common;

namespace EnglishAI.Domain.Entities;

public class UserSettings : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public bool NotificationEnabled { get; set; } = true;
    public TimeOnly? DailyReminderTime { get; set; }
    public bool SoundEnabled { get; set; } = true;
    public bool DarkMode { get; set; }
    public string LanguageUi { get; set; } = "vi";
    public bool AutoPlayAudio { get; set; } = true;
    public bool ShowPhonetic { get; set; } = true;

    public User User { get; set; } = null!;
}

