using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IListaBusiness : IBaseBusiness<Lista>
    {
        Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId);

        Task<bool> SetLista(SalvarListaDTO lista);
    }
}