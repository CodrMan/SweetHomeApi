using System.Linq;

using SweetHome.Core.Entities;
using SweetHome.Core.Interfaces;
using SweetHome.Core.Reflection;
using SweetHome.Core.Settings;
using SweetHome.Services.Abstract;


namespace SweetHome.Services.Concrete
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
