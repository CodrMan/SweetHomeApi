using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

using ApiAdmin.Core.Entities.Identity;
using ApiAdmin.Services.Concrete;


namespace ApiAdmin.Api.Helpers
{
    public class TokenGenerator
    {
        public static string GenerateToken(AppUserManager userManager, AppUser user)
        {
            ClaimsIdentity identity = userManager.CreateIdentity(user, Startup.OAuthBearerOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(700));
            return Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
        }
    }
}