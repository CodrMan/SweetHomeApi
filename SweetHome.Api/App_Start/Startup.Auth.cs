using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using Owin;

using SweetHome.Api.Providers;
using SweetHome.Data;
using SweetHome.Services.Concrete;


namespace SweetHome.Api
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new SimpleAuthorizationServerProvider()
            };
            
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalBearer);

            app.CreatePerOwinContext(DataDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
        }
    }
}