using System;

namespace ApiAdmin.Core.Messages
{
    [Serializable]
    public class EmailNotificationMessage : NotificationMessage
    {
        public string Subject { get; set; }
    }
}
