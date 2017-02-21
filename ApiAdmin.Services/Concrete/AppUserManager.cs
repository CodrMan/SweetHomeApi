using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin;

using ApiAdmin.Core.Entities.Identity;
using ApiAdmin.Data;
using ApiAdmin.Data.Identity;


namespace ApiAdmin.Services.Concrete
{
    public class AppUserManager : UserManager<AppUser, long>
    {
        public AppUserManager(AppUserStore store)
            : base(store)
        {
            var provider = new DpapiDataProtectionProvider("ApiAdmin");
            UserTokenProvider = new DataProtectorTokenProvider<AppUser, long>(provider.Create("EmailConfirmation"));
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new AppUserStore(context.Get<DataDbContext>()));

            return manager;
        }
    }
}
