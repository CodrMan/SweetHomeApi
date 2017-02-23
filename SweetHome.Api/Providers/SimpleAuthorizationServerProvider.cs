using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using SweetHome.Services.Concrete;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;

namespace SweetHome.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //hardocde
            context.OwinContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            //hardcode

            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //hardocde
            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            context.OwinContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
            //hardcode

            var manager = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            var user = await manager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}