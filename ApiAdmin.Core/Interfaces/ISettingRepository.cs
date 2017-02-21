using ApiAdmin.Core.Entities;

namespace ApiAdmin.Core.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>
    {
        Setting GetSettingParam(string settingName);
    }
}
