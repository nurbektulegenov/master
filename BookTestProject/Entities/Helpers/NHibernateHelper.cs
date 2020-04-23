using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace BookTestProject.Entities.Helpers
{
    public class NHibernateHelper1
    {

        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"Data Source=LENOVO-Y5070;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").ShowSql()
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