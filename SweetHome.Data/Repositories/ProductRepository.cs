using SweetHome.Core.Entities;
using SweetHome.Core.Interfaces;

namespace SweetHome.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(DataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
