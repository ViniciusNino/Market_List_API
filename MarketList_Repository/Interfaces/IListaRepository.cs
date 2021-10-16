using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IListaRepository : IBaseRepository<Lista>
    {
        Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId);
    }
}