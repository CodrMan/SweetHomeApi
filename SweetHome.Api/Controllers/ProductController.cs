using System.Web.Http;

using SweetHome.Api.Infrastructure;
using SweetHome.Services.Abstract;
using SweetHome.Services.Concrete;


namespace SweetHome.Api.Controllers
{
    [JsonWrapper]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(AppUserManager userManager, ISettingService settingService, IProductService productService) : base(userManager, settingService)
        {
            _productService = productService;
        }

        public IHttpActionResult GetProducts()
        {
            return Ok(_productService.GetAllItems());
        }
    }
}
