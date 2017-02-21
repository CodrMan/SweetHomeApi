using Microsoft.AspNet.Identity.EntityFramework;

using ApiAdmin.Core.Entities.Identity;


namespace ApiAdmin.Data.Identity
{
    public class AppUserStore : UserStore<AppUser, AppRole, long, AspNetUserLogin, AspNetRole, AspNetUserClaim>
    {
        public AppUserStore(DataDbContext context)
            : base(context)
        {
        }
    }
}
