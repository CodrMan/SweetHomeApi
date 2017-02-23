using System;

namespace SweetHome.Core.Messages
{
    [Serializable]
    public abstract class NotificationMessage
    {
        public short EntityType { get; set; }
        public long EntityId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Body { get; set; }
    }
}
