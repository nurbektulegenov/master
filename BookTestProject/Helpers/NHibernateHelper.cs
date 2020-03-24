using System;
using System.IO;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace BookTestProject.Helpers
{
    public class NHibernateHelper
    {
        public static ISessionFactory SessionFactory = CreateSessionFactory();
        protected static ISessionFactory CreateSessionFactory()
        {
            return new Configuration().Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.config")).BuildSessionFactory();
        }
        public static ISession CurrentSession
        {
            get => (ISession)HttpContext.Current.Items["current.session"];
            set => HttpContext.Current.Items["current.session"] = value;
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CurrentSession = SessionFactory.OpenSession();
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            CurrentSession?.Dispose();
        }
    }
}