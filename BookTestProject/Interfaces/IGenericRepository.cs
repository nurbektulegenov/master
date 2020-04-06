namespace BookTestProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        long GetBooksCount();
    }
}