using System.Net;

using ApiAdmin.Core.Reflection;


namespace ApiAdmin.Core.Settings
{
    public class SmtpServerCredentials
    {
        [SettingProperty("SmtpUseDefaultCredentials")]
        public bool UseDefaultCredential { get; set; }

        [SettingProperty("SmtpServerLogin")]
        public string UserName { get; set; }

        [SettingProperty("SmtpServerPass")]
        public string Password { get; set; }

        [SettingProperty("SmtpNotificationEmail")]
        public string SmtpNotificationEmail { get; set; }

        [SettingProperty("SmtpSenderName")]
        public string SmtpSenderName { get; set; }

        public NetworkCredential GetNetworkCredentials()
        {
            return new NetworkCredential(UserName, Password);
        }
    }
}
