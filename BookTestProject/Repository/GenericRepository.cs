using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BookTestProject.Entities;
using BookTestProject.Interfaces;
using NHibernate;
using NHibernate.Criterion;

namespace BookTestProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        internal DbSet<T> dbSet;

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

        public IQueryOver<T> Select(Expression<Func<T, object>> exp)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.QueryOver<T>().Select(Projections.Group<T>(exp));
            }
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