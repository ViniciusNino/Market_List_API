using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        Task AddAsync(TEntity obj);
        Task AddRangeAsync(ICollection<TEntity> obj);
        Task<TEntity> FindByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        void Update(TEntity obj);
        void UpdateRange(ICollection<TEntity> obj);
        Task RemoveAsync(int id);
        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
    }
}