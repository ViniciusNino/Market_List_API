using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<List<ItemDTO>> GetItemPorUnidade(int unidadeId);
    }
}