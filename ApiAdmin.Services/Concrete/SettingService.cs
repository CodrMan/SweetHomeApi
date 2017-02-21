using System.Linq;

using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Interfaces;
using ApiAdmin.Core.Reflection;
using ApiAdmin.Core.Settings;
using ApiAdmin.Services.Abstract;


namespace ApiAdmin.Services.Concrete
{
    public class SettingService : ServiceBase<Setting>, ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly ISettingsReflector _settingsReflector;

        public SettingService(ISettingRepository settingRepository, ISettingsReflector settingsReflector) : base(settingRepository)
        {
            _settingRepository = settingRepository;
            _settingsReflector = settingsReflector;
        }

        public Setting GetSettingParam(string settingName)
        {
            return _settingRepository.GetSettingParam(settingName);
        }

        public NotificationServiceSettings GetNotificationServiceSettings()
        {
            var notificationSettings = GetSettings<NotificationServiceSettings>();
            notificationSettings.SmtpCredentials = GetSettings<SmtpServerCredentials>();
            notificationSettings.SmtpSettings = GetSettings<SmtpServerSettings>();

            return notificationSettings;
        }

        public T GetSettings<T>() where T : new()
        {
            var keys = _settingsReflector.GetKeys<T>();
            var query = _settingRepository.GetAllItems().Where(x => keys.Contains(x.ParamName));
            var settings = query.ToDictionary(e => e.ParamName, e => e.ParamValue);

            return _settingsReflector.CreateNewObject<T>(settings);
        }
    }
}
