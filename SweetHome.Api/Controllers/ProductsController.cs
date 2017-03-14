using System.Web.Http;

using SweetHome.Core.Entities;
using SweetHome.Services.Abstract;
using SweetHome.Services.Concrete;


namespace SweetHome.Api.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(AppUserManager userManager, ISettingService settingService, IProductService productService) : base(userManager, settingService)
        {
            _productService = productService;
        }

        public IHttpActionResult GetProducts()
        {
            return Ok(_productService.GetAllItems());
        }

        public IHttpActionResult GetProduct(long id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();
            
            return Ok(product);
        }

        [HttpPut]
        public IHttpActionResult PutUser(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = new Product() { Id = product.Id, Name = product.Name };
            _productService.Update(newProduct);

            return Ok(newProduct);
        }
        
        [HttpPost]
        public IHttpActionResult PostUser(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _productService.Insert(product);
            return Ok();
        }
        
        [HttpDelete]
        public IHttpActionResult DeleteUser(long id)
        {
            _productService.Delete(id);
            return Ok();
        }
    }
}
