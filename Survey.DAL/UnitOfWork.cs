using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Survey.Models;
using System.Collections;

namespace Survey.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private Hashtable _repositories;

        public void Save()
        {
            context.SaveChanges();
        }

        public ApplicationDbContext GetContext()
        {
            return context;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(Repository<>);
            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context));

            return (IRepository<TEntity>)_repositories[type];
        }
    }
}
