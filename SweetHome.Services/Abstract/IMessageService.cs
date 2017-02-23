using System.Collections.Generic;

using SweetHome.Core.Entities;
using SweetHome.Core.Entities.Identity;
using SweetHome.Core.Enums;
using SweetHome.Core.Messages;


namespace SweetHome.Services.Abstract
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
