using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{    
    public class ListaBusiness : BaseBusiness<Lista>, IListaBusiness
    {
        private readonly IListaRepository _listaRepository;
        private readonly IItemListaRepository _itemListaRep;

        public ListaBusiness(IListaRepository listaReposytory, IItemListaRepository itemListaRep)
        {
            _listaRepository = listaReposytory;
            _itemListaRep = itemListaRep;
        }
        public async Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId)
        {
            return await _listaRepository.GetListaPorUnidadeId(unidadeId);
        }

        public async Task<bool> SetLista(SalvarListaDTO lista)
        {
            return await _listaRepository.SetLista(lista);
        }

        public async Task<bool> DeleteLista(int listaId)
        {
            await _itemListaRep.DeletarItensLista(listaId);
            var quantidadeListaDeletada = await _listaRepository.DeleteLista(listaId);
            return quantidadeListaDeletada > 0;
        }
    }
}