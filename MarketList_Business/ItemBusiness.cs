using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class ItemBusiness : BaseBusiness<Item>, IItemBusiness
    {
        private readonly IItemRepository _iItemRepository;
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public ItemBusiness(IItemRepository iItemRepository, IUnidadeDeTrabalho unidadeTrab) : base(iItemRepository, unidadeTrab)
        {
            _iItemRepository = iItemRepository;
            _unidadeTrab = unidadeTrab;
        }
        public async Task<List<ItemDTO>> GetItemPorUnidade(int unidadeId)
        {
            return await _iItemRepository.GetItemPorUnidade(unidadeId);
        }
    }
}