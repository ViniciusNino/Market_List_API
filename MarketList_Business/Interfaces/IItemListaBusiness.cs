using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IItemListaBusiness : IBaseBusiness<ItemLista>
    {
        Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId);
    }
}