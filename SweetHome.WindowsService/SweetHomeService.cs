using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using log4net;

using SweetHome.Core.Reflection;
using SweetHome.Data;
using SweetHome.Data.Repositories;
using SweetHome.Notifications;
using SweetHome.Services.Abstract;
using SweetHome.Services.Concrete;


namespace SweetHome.WindowsService
{
    partial class SweetHomeService : ServiceBase
    {
        private readonly DataDbContext _db = new DataDbContext();
        private readonly ISettingService _setting;
        private readonly ILog _log = LogManager.GetLogger(typeof(SweetHomeService));

        private int _intervalCleaningFolder;
        private int _intervalSendingNewMessages;
        private int _intervalSendingFailedMessages;

        private Timer _tempFoldersCleaningTimer;
        private Timer _sendNewMessagesTimer;
        private Timer _sendFailedMessagesTimer;

        public SweetHomeService()
        {
            _setting = InitSettingService();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _log.Info("Service has been started");
            Initialize(); 
        }

        protected override void OnStop()
        {
            _tempFoldersCleaningTimer.Stop();
            _sendNewMessagesTimer.Stop();
            _sendFailedMessagesTimer.Stop();
            _log.Info("Service has been stoped");
        }

        private void Initialize()
        {
            InitIntervals();

            _tempFoldersCleaningTimer = new Timer();
            _tempFoldersCleaningTimer.Interval = _intervalCleaningFolder;
            _tempFoldersCleaningTimer.Enabled = true;
            _tempFoldersCleaningTimer.Elapsed += OnClearTempDirectories;
            _tempFoldersCleaningTimer.AutoReset = true;

            _sendNewMessagesTimer = new Timer();
            _sendNewMessagesTimer.Interval = _intervalSendingNewMessages;
            _sendNewMessagesTimer.Enabled = true;
            _sendNewMessagesTimer.Elapsed += SendNewMessages;
            _sendNewMessagesTimer.AutoReset = true;

            _sendFailedMessagesTimer = new Timer();
            _sendFailedMessagesTimer.Interval = _intervalSendingFailedMessages;
            _sendFailedMessagesTimer.Enabled = true;
            _sendFailedMessagesTimer.Elapsed += SendFailedMessages;
            _sendFailedMessagesTimer.AutoReset = true;

            _tempFoldersCleaningTimer.Start();
            _sendNewMessagesTimer.Start();
            _sendFailedMessagesTimer.Start();
        }

        private void SendNewMessages(object sender, ElapsedEventArgs e)
        {
            _log.Info("SendNewMessages starting...");
            using (MessageProcessor messageProcessor = new MessageProcessor())
            {
                messageProcessor.SendNotSendedMessages();
            }           
        }

        private void SendFailedMessages(object sender, ElapsedEventArgs e)
        {
            _log.Info("SendFailedMessages starting...");
            using (MessageProcessor messageProcessor = new MessageProcessor())
            {
                messageProcessor.SendFailedMessages();
            }
        }

        private void OnClearTempDirectories(object sender, ElapsedEventArgs e)
        {
            _log.Info("Clear TempDirectories starting...");

            string appPath = _setting.GetSettingParam("AppPath").ParamValue;
            string tempFolder = "Content\\" + _setting.GetSettingParam("TempFolder").ParamValue;
            var pathToTemp = Path.Combine(appPath, tempFolder);

            if (Directory.Exists(pathToTemp))
            {
                var subdirectories = Directory.GetDirectories(pathToTemp);
                foreach (var subdirectory in subdirectories)
                {
                    var date = Directory.GetCreationTime(subdirectory);
                    var totalHours = (DateTime.Now - date).TotalHours;
                    if (totalHours > 1)
                    {
                        try
                        {
                            Directory.Delete(subdirectory, true);
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex.Message);
                        }
                    }
                }
            }

            _log.Info("Clear TempDirectories end work...");
        }

        private ISettingService InitSettingService()
        {
            var setting = new SettingService(new SettingRepository(_db), new SettingsReflector());
            return setting;
        }

        private void InitIntervals()
        {
            _log.Info("Starting initialization intervals...");
            int oneSecond = 1000;
            int clearTempFolderInterval;
            int pendingMessagesSendInterval;
            int notificationsSendInterval;

            if (int.TryParse(_setting.GetSettingParam("ClearTempFolderInterval").ParamValue, out clearTempFolderInterval)
                && int.TryParse(_setting.GetSettingParam("PendingMessagesSendInterval").ParamValue, out pendingMessagesSendInterval)
                && int.TryParse(_setting.GetSettingParam("NotificationsSendInterval").ParamValue, out notificationsSendInterval))
            {
                _intervalCleaningFolder = oneSecond * clearTempFolderInterval;
                _intervalSendingFailedMessages = oneSecond * pendingMessagesSendInterval;
                _intervalSendingNewMessages = oneSecond * notificationsSendInterval;
                _log.Info("Initialization intervals - success!");
            }
            else
            {
                _log.Error("Geting settings for timer intervals - failed!");
            }
        }
    }
}
