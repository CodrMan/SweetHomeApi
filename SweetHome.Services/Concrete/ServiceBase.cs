using System.Collections.Generic;

using SweetHome.Core.Entities;
using SweetHome.Core.Interfaces;
using SweetHome.Services.Abstract;


namespace SweetHome.Services.Concrete
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;

        public ServiceBase(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public IEnumerable<TEntity> GetAllItems()
        {
            return _repository.GetAllItems();
        }

        public TEntity GetById(long id)
        {
            return _repository.GetById(id);
        }

        public void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
