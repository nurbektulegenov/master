using BookTestProject.Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace BookTestProject.Helpers
{
    public static class FluentNHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get
            {
                if(_sessionFactory == null)
                {
                    var dbConfig = MsSqlConfiguration.MsSql2008
                        .ConnectionString(c => c.FromConnectionStringWithKey("DBConnection"))
                        .ShowSql();

                    _sessionFactory = Fluently.Configure()
                        .Database(dbConfig)
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
                        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true)) 
                        .BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}