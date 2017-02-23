using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using SweetHome.Core.Entities;
using SweetHome.Core.Messages;


namespace SweetHome.Notifications
{
    public class NotificationMessageContainer
    {
        public NotificationMessage Message { get; }

        public long MessageId { get; private set; }

        public long? UserId { get; private set; }

        private NotificationMessageContainer(NotificationMessage message)
        {
            Message = message;
        }

        public static NotificationMessageContainer Create(Message message)
        {
            var mess = ReadBinaryData(message.SerializedMessage);

            if (mess == null)
                throw new SerializationException(string.Format("Message with Id \"{0}\" has invalid serialized data.", message.Id));

            return new NotificationMessageContainer(mess)
            {
                UserId = message.AspNetUser.Id,
                MessageId = message.Id
            };
        }

        private static NotificationMessage ReadBinaryData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return null;

            using (var stream = new MemoryStream())
            {
                try
                {
                    stream.Write(data, 0, data.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    var formatter = new BinaryFormatter();
                    return (NotificationMessage)formatter.Deserialize(stream);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
