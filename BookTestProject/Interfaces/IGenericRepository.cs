using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BookTestProject.Entities;

namespace BookTestProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        T Load(int id);
        List<TRes> Select<TRes>(Expression<Func<T, TRes>> exp);
        List<TRes> SelectBooks<TRes>(Expression<Func<T, TRes>> exp, Expression<Func<TRes, int>> orderExp, int skip, int take);
        List<T> Where(Expression<Func<T, bool>> exp);
        T Find(Expression<Func<T, bool>> exp);
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        long GetBooksCount();
        void SoftDelete(Books book);
    }
}