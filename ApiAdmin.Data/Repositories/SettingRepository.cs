using System.Linq;

using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Interfaces;


namespace ApiAdmin.Data.Repositories
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
