using System;
using System.Net.Mail;
using Microsoft.AspNet.Identity;

using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Messages;
using ApiAdmin.Core.Settings;
using ApiAdmin.Data;
using ApiAdmin.Data.Identity;
using ApiAdmin.Services.Concrete;


namespace ApiAdmin.Notifications
{
    public class MessageSender : IDisposable
    {
        private static DataDbContext _dbcontext;
        private static DataDbContext DbContext
        {
            get
            {
                _dbcontext = _dbcontext ?? new DataDbContext();
                return _dbcontext;
            }
        }

        public static SendResult SendMessage(NotificationServiceSettings settings, Message message)
        {
            var container = NotificationMessageContainer.Create(message);
            var client = CreateSmtpClient(settings);
            var mailMessage = CreateEmailMessage(settings, container);

            try
            {
                client.Send(mailMessage);
                return new SendResult();
            }
            catch (Exception e)
            {
                return new SendResult(e);
            }
        }

        private static SmtpClient CreateSmtpClient(NotificationServiceSettings settings)
        {
            var client = new SmtpClient(settings.SmtpSettings.ServerName, settings.SmtpSettings.ServerPort)
            {
                UseDefaultCredentials = settings.SmtpCredentials.UseDefaultCredential
            };

            if (!settings.SmtpCredentials.UseDefaultCredential)
                client.Credentials = settings.SmtpCredentials.GetNetworkCredentials();

            client.EnableSsl = settings.SmtpSettings.UseSSL;
            return client;
        }

        private static MailMessage CreateEmailMessage(NotificationServiceSettings settings, NotificationMessageContainer messageContainer)
        {
            var userMapanger = new AppUserManager(new AppUserStore(DbContext));
            var messageToSend = (EmailNotificationMessage)messageContainer.Message;
            var user = userMapanger.FindById(messageContainer.UserId.Value);

            return new MailMessage
            {
                From = new MailAddress(settings.SmtpCredentials.SmtpNotificationEmail, settings.SmtpCredentials.SmtpSenderName),
                To = { user.Email },
                Subject = messageToSend.Subject,
                Body = messageToSend.Body,
                IsBodyHtml = false
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
