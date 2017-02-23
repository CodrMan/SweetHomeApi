using SweetHome.Core.Entities;

namespace SweetHome.Services.Abstract
{
    public interface ISettingService : IServiceBase<Setting>
    {
        Setting GetSettingParam(string settingName);
    }
}
