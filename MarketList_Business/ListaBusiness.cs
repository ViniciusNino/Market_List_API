using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{    
    public class ListaBusiness : BaseBusiness<Lista>, IListaBusiness
    {
        private readonly IListaRepository _listaRepository;

        public ListaBusiness(IListaRepository listaReposytory)
        {
            _listaRepository = listaReposytory;
        }
        public async Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId)
        {
            return await _listaRepository.GetListaPorUnidadeId(unidadeId);
        }

        public async Task<bool> SetLista(SalvarListaDTO lista)
        {
            return await _listaRepository.SetLista(lista);
        }
    }
}