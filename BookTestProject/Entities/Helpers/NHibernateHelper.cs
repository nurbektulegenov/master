using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace BookTestProject.Entities.Helpers
{
    public class NHibernateHelper
    {

        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"data source=(localdb)\MSSQLLocalDB;Initial Catalog=Books_test;Integrated Security=True;").ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Books>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Authors>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TotalCounts>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}