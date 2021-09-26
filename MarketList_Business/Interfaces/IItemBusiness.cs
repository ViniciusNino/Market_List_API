using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IItemBusiness : IBaseBusiness<Item>
    {
        Task<List<ItemViewModel>> GetItemPorUnidade(int unidadeId);
    }
}