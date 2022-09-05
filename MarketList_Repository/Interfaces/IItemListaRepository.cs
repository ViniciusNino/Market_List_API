using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IItemListaRepository : IBaseRepository<ItemLista>
    {
        Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId);
        Task<List<ItemLista>> ListarItensListaAtualizar(List<int?> idsAtualizar);
        Task<int> AtualizarItensLista(List<ItemLista> itensAtualizar, List<int?> idsExcluir);
        Task<int> DeletarItensLista(int listaId);
    }
}