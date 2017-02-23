using System;

namespace SweetHome.Core.Messages
{
    [Serializable]
    public class EmailNotificationMessage : NotificationMessage
    {
        public string Subject { get; set; }
    }
}
