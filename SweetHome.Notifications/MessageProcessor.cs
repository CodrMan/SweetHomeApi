using System;
using System.Collections.Generic;

using SweetHome.Core.Entities;
using SweetHome.Core.Enums;
using SweetHome.Core.Messages;
using SweetHome.Core.Reflection;
using SweetHome.Core.Settings;
using SweetHome.Data;
using SweetHome.Data.Identity;
using SweetHome.Data.Repositories;
using SweetHome.Services.Concrete;


namespace SweetHome.Notifications
{
    public class MessageProcessor : IDisposable
    {
        private DataDbContext _dbcontext;
        private DataDbContext DbContext
        {
            get
            {
                _dbcontext = _dbcontext ?? new DataDbContext();
                return _dbcontext;
            }
        }
        public NotificationServiceSettings Settings { get; }

        public MessageProcessor()
        {
            Settings = GetSettings();
        }

        public void SendNotSendedMessages()
        {
            SendMessages(MessageState.NotSended);
        }

        public void SendFailedMessages()
        {
            SendMessages(MessageState.Pending);
        }

        private void SendMessages(MessageState state)
        {
            var messageRepository = new MessageRepository(DbContext);
            var userManager = new AppUserManager(new AppUserStore(DbContext));
            var messageService = new MessageService(userManager, messageRepository);
            var messages = messageService.GetMessagesForSend(state);
            var resultsMap = new Dictionary<SendResult, Message>();

            foreach (var message in messages)
            {
                var result = MessageSender.SendMessage(Settings, message);
                resultsMap.Add(result, message);
            }

            messageService.SetMessagesState(resultsMap);
        }

        private NotificationServiceSettings GetSettings()
        {
            var settingService = new SettingService(new SettingRepository(DbContext), new SettingsReflector());
            return settingService.GetNotificationServiceSettings();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
