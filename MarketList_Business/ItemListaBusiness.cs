using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class ItemListaBusiness : BaseBusiness<ItemLista>, IItemListaBusiness
    {
        private readonly IItemListaRepository _itemListaRepository;

        public ItemListaBusiness(IItemListaRepository itemListaRepository)
        {
            _itemListaRepository = itemListaRepository;
        } 
        public Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId)
        {
            return _itemListaRepository.GetGetItensListaPorListaId(listaId);
        }
    }
}