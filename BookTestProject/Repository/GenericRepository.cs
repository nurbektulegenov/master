using BookTestProject.Entities;
using BookTestProject.Interfaces;
using NHibernate;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BookTestProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ISession _session;

        public GenericRepository(ISession session)
        {
            _session = session;
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

        public IQueryable<TRes> Select<TRes>(Expression<Func<T, TRes>> expression)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Query<T>().Select(expression);
            }
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Query<T>().Where(expression);
            }
        }
        public T Find(Expression<Func<T, bool>> expression)
        {
            return Where(expression).FirstOrDefault();
        }

        public T Load(int id)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Load<T>(id);
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