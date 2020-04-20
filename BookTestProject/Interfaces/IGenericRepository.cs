using System;

namespace BookTestProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        T Load(int id);
        T Select(Func<T> exp);
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        long GetBooksCount();
    }
}