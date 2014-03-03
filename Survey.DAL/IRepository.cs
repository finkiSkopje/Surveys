using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Survey.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQuery(string includeProperties = "");
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string orderByGeneric = null,
            int pageIndex = 0, int pageSize = 0,
            string includeProperties = "");

        TEntity GetById(object id);

        void Insert(TEntity item);

        void Delete(object id);

        void Delete(TEntity item);

        void Update(TEntity item);
    }
}
