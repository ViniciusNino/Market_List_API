using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity<TEntity>
    {
        protected readonly MarketListContext Db;
        protected DbSet<TEntity> DbSet;

        public BaseRepository()
        { }

        public BaseRepository(MarketListContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity obj)
        {
            await DbSet.AddAsync(obj);
        }

        public virtual void Update(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
            DbSet.Update(obj);
        }

        public virtual async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }

        public virtual async Task RemoveAsync(int id)
        {
            DbSet.Remove(await DbSet.FindAsync(id));
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}