using SweetHome.Core.Entities;

namespace SweetHome.Core.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>
    {
        Setting GetSettingParam(string settingName);
    }
}
