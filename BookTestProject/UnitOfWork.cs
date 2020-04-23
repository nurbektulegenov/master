using BookTestProject.Entities;
using BookTestProject.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;

namespace BookTestProject
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ITransaction _transaction;
        public ISession Session { get; private set; }

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            Session = _sessionFactory.OpenSession();
            Session.FlushMode = FlushMode.Auto;
            _transaction = Session.BeginTransaction();
        }

        public void Dispose()
        {
            if (Session.IsOpen)
            {
                Session.Close();
                Session = null;
            }
        }

        public void Commit()
        {
            if (!_transaction.IsActive)
            {
                throw new InvalidOperationException("Нет активных транзакций");
            }
            _transaction.Commit();
        }

        public void Rollback()
        {
            if (_transaction.IsActive)
            {
                _transaction.Rollback();
            }
        }
        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"Data Source=LENOVO-Y5070; Initial Catalog=Books_test;Integrated Security=True;").ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Books>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Authors>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TotalCounts>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}