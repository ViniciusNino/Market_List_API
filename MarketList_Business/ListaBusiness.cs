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
        public Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId)
        {
            return _listaRepository.GetListaPorUnidadeId(unidadeId);
        }
    }
}