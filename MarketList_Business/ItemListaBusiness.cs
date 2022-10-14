using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Model.DTO;
using MarketList_Repository;

namespace MarketList_Business
{
    public class ItemListaBusiness : BaseBusiness<ItemLista>, IItemListaBusiness
    {
        private readonly IItemListaRepository _itemListaRepository;
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public ItemListaBusiness(IItemListaRepository itemListaRepository, IUnidadeDeTrabalho unidadeTrab) : base(itemListaRepository, unidadeTrab)
        {
            _itemListaRepository = itemListaRepository;
            _unidadeTrab = unidadeTrab;
        }

        public Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId)
        {
            return _itemListaRepository.GetGetItensListaPorListaId(listaId);
        }

        public async Task<int> AtualizarItensLista(ListaAtualizarDTO listaAtualizarDto)
        {
            var IdsExcluir = listaAtualizarDto.ItensLista.Where(x => x.Quantidade == null).Select(x => x.Id).ToList();
            var IdsAtualizar = listaAtualizarDto.ItensLista.Where(x => x.Quantidade != null && x.Id != null).Select(x => x.Id).ToList();
            var itensAtualizar = await _itemListaRepository.ListarItensListaAtualizar(IdsAtualizar);
            itensAtualizar = AtualizarQuantidade(itensAtualizar, listaAtualizarDto.ItensLista);
            itensAtualizar.AddRange(CriarItensAdd(listaAtualizarDto));


            return await _itemListaRepository.AtualizarItensLista(itensAtualizar, IdsExcluir);
        }

        private List<ItemLista> CriarItensAdd(ListaAtualizarDTO listaAtualizarDto)
        {
            var itensAddDto = listaAtualizarDto.ItensLista.Where(x => x.Id == null).ToList();
            var intesAdd = new List<ItemLista>();

            foreach (var item in itensAddDto)
            {
                var itemAdd = new ItemLista()
                {
                    BAtivo = true,
                    DCadastro = DateTime.Now,
                    NIdLista = listaAtualizarDto.ListaId,
                    NIdItem = item.ItemId ?? 0,
                    NQuantidade = item.Quantidade ?? 0,
                    NIdStatus = (int)EStatusItemLista.Solicitado,
                    NIdUsuarioSolicitante = listaAtualizarDto.UsuarioLogadoId
                };

                intesAdd.Add(itemAdd);
            }


            return intesAdd;
        }

        private List<ItemLista> AtualizarQuantidade(List<ItemLista> itensAtualizar, List<ItemListaAtualizarDTO> itensLista)
        {
            foreach (var item in itensAtualizar)
            {
                item.NQuantidade = itensLista.Where(x => x.Id == item.Id).FirstOrDefault().Quantidade ?? 0;
            }

            return itensAtualizar;
        }
    }
}