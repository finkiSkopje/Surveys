using Survey.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace Survey.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext _context;
        internal DbSet<TEntity> dbSet;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetQuery(string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string orderByGeneric = null,
            int pageIndex = 0, int pageSize = 0,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (!string.IsNullOrEmpty(orderByGeneric))
            {
                query = query.OrderBy(orderByGeneric);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageSize > 0)
            {
                return query.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity item)
        {
            dbSet.Add(item);
        }

        public virtual void Delete(object id)
        {
            TEntity item = dbSet.Find(id);
            Delete(item);
        }

        public virtual void Delete(TEntity item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }
            dbSet.Remove(item);
        }

        public virtual void Update(TEntity item)
        {
            dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
