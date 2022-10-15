using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class BaseBusiness<TEntity> : IBaseBusiness<TEntity> where TEntity : Entity<TEntity>
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public BaseBusiness()
        { }

        public BaseBusiness(IBaseRepository<TEntity> repository, IUnidadeDeTrabalho unidadeTrab)
        {
            _repository = repository;
            _unidadeTrab = unidadeTrab;
        }

        public async Task<TEntity> AddAsync(TEntity obl)
        {
            if (!obl.IsValid())
            {
                return null;
            }

            await _repository.AddAsync(obl);
            await _unidadeTrab.CommitAsync();

            return obl;
        }

        public async Task UpdateAsync(TEntity user)
        {
            _repository.Update(user);
            await _unidadeTrab.CommitAsync();
        }

        public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }

        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await _repository.GetAllAsync(filter, includes);
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await _repository.GetFirstOrDefaultAsync(filter, includes);
        }

        public virtual async Task RemoveAsync(int id)
        {
            await _repository.RemoveAsync(id);
            await _unidadeTrab.CommitAsync();
        }
    }
}