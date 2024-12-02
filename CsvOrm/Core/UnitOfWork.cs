using System;

namespace CsvOrm.Core
{
    public sealed class UnitOfWork : IDisposable
    {
        private readonly CsvContext context;
        private bool disposed;

        public UnitOfWork()
        {
            context = new CsvContext();
        }

        public Repository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            return context.GetRepository<TEntity>();
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }
    }
}