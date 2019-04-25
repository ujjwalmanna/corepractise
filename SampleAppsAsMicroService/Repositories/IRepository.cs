using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SampleAppsAsMicroService.Repositories
{
    public interface IRepository<T, in TKey> where T:class
    {
        T Add(T entity);
        void Delete(T entity);
        void DeleteByKey(TKey id);
        void Update(T entity);
        T Get(TKey id);
        ICollection<T> SearchResult(Expression<Func<T, bool>> expr);
        ICollection<T> All();
    }
}
