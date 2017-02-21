using Microsoft.AspNet.Identity.EntityFramework;

using ApiAdmin.Core.Entities.Identity;


namespace ApiAdmin.Data.Identity
{
    public class AppRoleStore : RoleStore<AppRole, long, AspNetRole>
    {
        public AppRoleStore(DataDbContext context) : base(context)
        {
        }
    }
}
