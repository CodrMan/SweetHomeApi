using Microsoft.AspNet.Identity.EntityFramework;

using SweetHome.Core.Entities.Identity;


namespace SweetHome.Data.Identity
{
    public class AppRoleStore : RoleStore<AppRole, long, AspNetRole>
    {
        public AppRoleStore(DataDbContext context) : base(context)
        {
        }
    }
}
