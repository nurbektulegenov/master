using System.Linq;
using BookTestProject.Entities;
using BookTestProject.Interfaces;
using NHibernate;

namespace BookTestProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual void Add(T entity)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
        }

        public virtual void Update(T entity)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }

        public virtual void Delete(int id)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(session.Load<T>(id));
                    transaction.Commit();
                }
            }
        }

        public T GetById(int id)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public virtual long GetBooksCount()
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Query<TotalCounts>().Select(a => a.BooksCount).FirstOrDefault();
            }
        }
    }
}