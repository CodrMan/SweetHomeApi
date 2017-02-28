using SweetHome.Core.Entities;

namespace SweetHome.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(DataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
