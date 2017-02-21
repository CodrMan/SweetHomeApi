using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ApiAdmin.Core.Entities;


namespace ApiAdmin.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        List<TEntity> GetAllItems();
        TEntity GetById(long id);
        TEntity Get(Expression<Func<TEntity, bool>> @where);
        TEntity Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(long id);
        void SaveChanges();
    }
}
