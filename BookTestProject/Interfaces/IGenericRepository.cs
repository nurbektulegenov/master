using System;
using System.Linq;
using System.Linq.Expressions;

namespace BookTestProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        T Load(int id);
        IQueryable<TRes> Select<TRes>(Expression<Func<T, TRes>> exp);
        IQueryable<T> Where(Expression<Func<T, bool>> exp);
        T Find(Expression<Func<T, bool>> exp);
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        long GetBooksCount();
    }
}