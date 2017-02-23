using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

using SweetHome.Core.Entities.Identity;
using SweetHome.Api.Infrastructure;
using SweetHome.Services.Abstract;
using SweetHome.Services.Concrete;
using log4net;

namespace SweetHome.Api.Controllers
{
    [RequestLogging]
    public class BaseController : ApiController
    {
        public const string HttpStateKey = "RequestHttpStateKey";
        public const string HttpMessageKey = "RequestHttpMessageKey";

        protected readonly ILog Log = LogManager.GetLogger(typeof(ApiController));
        protected readonly ISettingService SettingService;
        protected readonly AppUserManager UserManager;

        private AppUser _currentUser;
        public AppUser CurrentUser
        {
            get
            {
                if (_currentUser == null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var userIdentity = HttpContext.Current.GetOwinContext().Authentication.User.Identity;
                    if (userIdentity.IsAuthenticated)
                    {
                        var name = HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;
                        if (name != null)
                        {
                            _currentUser = UserManager.FindByName(name);
                        }
                    }
                }
                return _currentUser;
            }
        }

        public BaseController(AppUserManager userManager, ISettingService settingService)
        {
            SettingService = settingService;
            UserManager = userManager;
        }

        public void HttpCode(HttpStatusCode code)
        {
            HttpContext.Current.Items[HttpStateKey] = code;
        }

        public void HttpMessage(string message)
        {
            HttpContext.Current.Items[HttpMessageKey] = message;
        }

        protected string GetErrorResult(IEnumerable<string> errors)
        {
            var str = "";
            foreach (var error in errors)
            {
                str = error + "\r\n";
            }

            return str;
        }
    }
}
