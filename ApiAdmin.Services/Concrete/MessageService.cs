using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Runtime.Serialization.Formatters.Binary;

using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Entities.Identity;
using ApiAdmin.Core.Enums;
using ApiAdmin.Core.Interfaces;
using ApiAdmin.Core.Messages;
using ApiAdmin.Services.Abstract;


namespace ApiAdmin.Services.Concrete
{
    public class MessageService : ServiceBase<Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly AppUserManager _userManager;

        public MessageService(AppUserManager userManager, IMessageRepository messageRepository) : base(messageRepository)
        {
            _userManager = userManager;
            _messageRepository = messageRepository;
        }

        public void AddErrorMessage(string error)
        {
            _messageRepository.Insert(new Message
            {
                AspNetUser = _userManager.FindById(1), //HardCode
                AttemptsCount = 0,
                CreateDate = DateTime.Now,
                MessageState = (short)MessageState.NotSended,
                SerializedMessage = CreateErrorMessage(error)
            });
        }

        public List<Message> GetMessagesForSend(MessageState state)
        {
            return _messageRepository.GetAllItems().Where(x => x.MessageState == (short)state).ToList();
        }

        public void SetMessagesState(Dictionary<SendResult, Message> messages)
        {
            foreach (var messagesState in messages)
            {
                UpdateMessageState(messagesState.Value, messagesState.Key);
            }
        }

        public void AddRestorePasswordMessage(AppUser user, string newPassword)
        {
            _messageRepository.Insert(new Message
            {
                AspNetUser = user,
                AttemptsCount = 0,
                CreateDate = DateTime.Now,
                MessageState = (short)MessageState.NotSended,
                SerializedMessage = CreateRestorePasswordMessage(newPassword)
            });
        }

        public void DeleteAllByUserId(long userId)
        {
            var flag = true;
            while (flag)
            {
                var mess = _messageRepository.Get(x => x.AspNetUser.Id == userId);
                if (mess == null)
                    flag = false;
                else
                    _messageRepository.Delete(mess.Id);
            } 
        }

        private void UpdateMessageState(Message message, SendResult result)
        {
            if (result.Success)
            {
                message.MessageState = (short)MessageState.Sended;
                message.ExceptionMessage = null;
            }
            else
            {
                message.MessageState = (short)MessageState.Pending;
                message.AttemptsCount++;
                message.ExceptionMessage = result.Error;
            }

            message.UpdateDate = DateTime.Now;
            Update(message);
        }

        private byte[] CreateRestorePasswordMessage(string newPassword)
        {
            var message = new EmailNotificationMessage
            {
                Body = string.Format("Your new password is: {0}", newPassword),
                Subject = "ApiAdmin Service - Restore Password"
            };

            return GetMessageBytes(message);
        }

        private byte[] CreateErrorMessage(string error)
        {
            var message = new EmailNotificationMessage
            {
                Body = error,
                Subject = "ApiAdmin Service - Error"
            };

            return GetMessageBytes(message);
        }

        private byte[] GetMessageBytes(object message)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, message);
                return stream.ToArray();
            }
        }
    }
}
