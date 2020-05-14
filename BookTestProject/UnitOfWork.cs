using BookTestProject.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using BookTestProject.Mapping;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;

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
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BookMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AuthorMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TotalCountsMap>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .ExposeConfiguration(x=>x.SetListener(ListenerType.Delete, new SoftDeleteEventListener()))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}