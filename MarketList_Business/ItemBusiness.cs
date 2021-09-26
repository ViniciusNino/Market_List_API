using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class ItemBusiness : BaseBusiness<Item>, IItemBusiness
    {
        private readonly IItemRepository _iItemRepository;
        public ItemBusiness(IItemRepository iItemRepository) : base (iItemRepository)
        {
            _iItemRepository = iItemRepository;
        }
        public async Task<List<ItemViewModel>> GetItemPorUnidade(int unidadeId)
        {
            return await _iItemRepository.GetItemPorUnidade(unidadeId);
        }
    }
}