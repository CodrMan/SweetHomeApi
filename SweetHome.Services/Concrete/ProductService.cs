using SweetHome.Core.Entities;
using SweetHome.Core.Interfaces;
using SweetHome.Services.Abstract;

namespace SweetHome.Services.Concrete
{
    public class ProductService : ServiceBase<Product>, IProductService
    {
        public ProductService(IRepository<Product> repository) : base(repository)
        {
        }
    }
}
