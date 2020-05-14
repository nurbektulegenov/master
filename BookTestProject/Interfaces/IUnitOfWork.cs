using System;
using System.Data.Entity;
using NHibernate;

namespace BookTestProject.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get; }
        void Commit();
        void Rollback();
    }
}