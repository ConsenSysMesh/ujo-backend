using System;
using System.Data;

namespace Ujo.Repository.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}