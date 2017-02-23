using System.Linq;

using SweetHome.Core.Entities;
using SweetHome.Core.Interfaces;


namespace SweetHome.Data.Repositories
{
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository 
    {
        public SettingRepository(DataDbContext dbContext) : base(dbContext)
        {
        }

        public Setting GetSettingParam(string settingName)
        {
            return Dbset.FirstOrDefault(x => x.ParamName == settingName);
        }
    }
}
