using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IListaRepository : IBaseRepository<Lista>
    {
        Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId);
        Task<Lista> SetLista(Lista lista);
        Task<int> DeleteLista(int listaId);
        Task<int> SalvarAgrupado(Agrupador agrupados);
        Task<int> SalvarAgrupadosListas(List<ListaAgrupador> lista);
        Task<List<ItensListasAgrupadasDTO>> GetListaAgrupadas(int usuarioId);
    }
}