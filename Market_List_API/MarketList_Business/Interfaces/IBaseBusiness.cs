using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IBaseBusiness<TEntity> where TEntity : Entity<TEntity>
    {
        Task<TEntity> AddAsync(TEntity obl);
        Task UpdateAsync(TEntity user);
        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
        Task RemoveAsync(int id);
    }
}