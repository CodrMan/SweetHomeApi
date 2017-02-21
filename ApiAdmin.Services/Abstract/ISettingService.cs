using ApiAdmin.Core.Entities;

namespace ApiAdmin.Services.Abstract
{
    public interface ISettingService : IServiceBase<Setting>
    {
        Setting GetSettingParam(string settingName);
    }
}
