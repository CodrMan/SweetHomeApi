using System.Collections.Generic;

namespace SweetHome.Services.Abstract
{
    public interface IServiceBase<TEntity>
    {
        IEnumerable<TEntity> GetAllItems();
        TEntity GetById(long id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(long id);
    }
}
