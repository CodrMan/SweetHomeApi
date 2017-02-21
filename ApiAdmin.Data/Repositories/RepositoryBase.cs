using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;

using ApiAdmin.Core.Entities;
using ApiAdmin.Core.Interfaces;


namespace ApiAdmin.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DataDbContext _dataContext;

        public IDbSet<TEntity> Dbset { get { return _dataContext.Set<TEntity>(); } }

        public RepositoryBase(DataDbContext dbContext)
        {
            _dataContext = dbContext;
        }

        public List<TEntity> GetAllItems()
        {
            return Dbset.ToList();
        }

        public TEntity GetById(long id)
        {
            return Dbset.FirstOrDefault(e => e.Id == id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> @where)
        {
            return Dbset.Where(where).FirstOrDefault();
        }

        public TEntity Insert(TEntity entity)
        {
            var result = Dbset.Add(entity);
            SaveChanges();
            return result;
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            Dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            SaveChanges();
        }

        public void Delete(long id)
        {
            var entity = GetById(id);
            if (entity == null)
                throw new ObjectNotFoundException("entity");
            Dbset.Remove(entity);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
    }
}
