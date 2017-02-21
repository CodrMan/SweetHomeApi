using System.Collections.Generic;

using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Entities.Identity;
using ApiAdmin.Core.Enums;
using ApiAdmin.Core.Messages;


namespace ApiAdmin.Services.Abstract
{
    public interface IMessageService : IServiceBase<Message>
    {
        void AddRestorePasswordMessage(AppUser user, string newPassword);
        void AddErrorMessage(string error);
        List<Message> GetMessagesForSend(MessageState state);
        void SetMessagesState(Dictionary<SendResult, Message> messages);
        void DeleteAllByUserId(long userId);
    }
}
