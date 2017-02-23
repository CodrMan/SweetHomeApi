using Microsoft.AspNet.Identity.EntityFramework;

using SweetHome.Core.Entities.Identity;


namespace SweetHome.Data.Identity
{
    public class AppUserStore : UserStore<AppUser, AppRole, long, AspNetUserLogin, AspNetRole, AspNetUserClaim>
    {
        public AppUserStore(DataDbContext context)
            : base(context)
        {
        }
    }
}
