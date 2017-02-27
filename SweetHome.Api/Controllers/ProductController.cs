using System.Collections.Generic;
using System.Web.Http;

using SweetHome.Api.Infrastructure;
using SweetHome.Api.Models;
using SweetHome.Services.Abstract;
using SweetHome.Services.Concrete;

namespace SweetHome.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [JsonWrapper]
    public class ProductController : BaseController
    {
        public ProductController(AppUserManager userManager, ISettingService settingService) : base(userManager, settingService)
        {
        }

        public IEnumerable<ProductViewModel> GetProducts()
        {
            return new List<ProductViewModel>();
        }
    }
}
