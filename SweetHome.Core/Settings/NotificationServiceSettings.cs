using SweetHome.Core.Reflection;

namespace SweetHome.Core.Settings
{
    public class NotificationServiceSettings
    {
        [SettingProperty("NotificationsSendInterval")]
        public long NotificationsSendInterval { get; set; }

        [SettingProperty("PendingMessagesSendInterval")]
        public long PendingMessagesSendInterval { get; set; }

        public SmtpServerSettings SmtpSettings { get; set; }
        public SmtpServerCredentials SmtpCredentials { get; set; }
    }
}
