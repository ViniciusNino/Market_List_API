using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IItemListaRepository : IBaseRepository<ItemLista>
    {
        Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId);
    }
}