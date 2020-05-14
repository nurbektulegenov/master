using BookTestProject.Entities;
using BookTestProject.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookTestProject.Models;
using NHibernate.Criterion;

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

        public List<TRes> Select<TRes>(Expression<Func<T, TRes>> expression)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Query<T>().Select(expression).ToList();
            }
        }

        public List<TRes> SelectBooks<TRes>(Expression<Func<T, TRes>> expression, Expression<Func<TRes, int>> orderExp, int skip, int take)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Query<T>().Select(expression).OrderBy(orderExp).Skip(skip).Take(take).ToList();
            }
        }

        public List<T> Where(Expression<Func<T, bool>> expression)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                return session.Query<T>().Where(expression).ToList();
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
        public void SoftDelete(Books book)
        {
            using (ISession session = UnitOfWork.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    book.IsDeleted = true;
                    session.Update(book);
                    transaction.Commit();
                }
            }
        }
    }
}