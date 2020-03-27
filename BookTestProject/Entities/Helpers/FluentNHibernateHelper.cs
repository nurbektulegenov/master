using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace BookTestProject.Entities.Helpers
{
    public class FluentNHibernateHelper
    {
        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"data source=(localdb)\MSSQLLocalDB;Initial Catalog=Books_test;Integrated Security=True;").ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Books>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Authors>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}