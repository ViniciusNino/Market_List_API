using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IBaseBusiness<TEntity> where TEntity : Entity<TEntity>
    {
        Task<TEntity> AddAsync(TEntity obl);

    }
}