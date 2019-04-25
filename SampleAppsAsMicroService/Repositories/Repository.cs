using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SampleAppsAsMicroService.Efs;

namespace SampleAppsAsMicroService.Repositories
{
    public class Repository<T,TKey> : IRepository<T,TKey>
        where T:class
    {
        private readonly EfContext _dbContext;
        public Repository(EfContext conext )
        {
            _dbContext = conext;
        }
        public T Add(T entity)
        {
            GetEntityDbSetSet().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        public ICollection<T> All()
        {
            return GetEntityDbSetSet().ToList();
        }
        public void Delete(T entity)
        {
            GetEntityDbSetSet().Remove(entity);
            _dbContext.SaveChanges();
        }
        public void Update(T entity)
        {
            GetEntityDbSetSet().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public T Get(TKey id)
        {
           return GetEntityDbSetSet().Find(id);
        }
        public ICollection<T> SearchResult(Expression<Func<T, bool>> expr)
        {
            return GetEntityDbSetSet().Where(expr).ToList();
        }
        protected DbSet<T> GetEntityDbSetSet()
        {
            return _dbContext.Set<T>();
        }
        public void DeleteByKey(TKey id)
        {
            var entity = GetEntityDbSetSet().Find(id);
            GetEntityDbSetSet().Remove(entity);
            _dbContext.SaveChanges();
        }
    }
   
}
