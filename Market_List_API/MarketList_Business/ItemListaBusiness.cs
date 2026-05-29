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
            itensAtualizar = AtualizarValores(itensAtualizar, listaAtualizarDto.ItensLista, listaAtualizarDto.UsuarioLogadoId);
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
                    NPreco = item.Preco,
                    NIdStatus = item.StatusItemListaId ?? (int)StatusItemListaEnum.Solicitado,
                    NIdUsuarioSolicitante = listaAtualizarDto.UsuarioLogadoId
                };

                intesAdd.Add(itemAdd);
            }

            return intesAdd;
        }

        private List<ItemLista> AtualizarValores(List<ItemLista> itensAtualizar, List<ItemListaAtualizarDTO> itensLista, int usuarioLogadoId)
        {
            foreach (var item in itensAtualizar)
            {
                var dto = itensLista.FirstOrDefault(x => x.Id == item.Id);
                if (dto != null)
                {
                    item.NQuantidade = dto.Quantidade ?? 0;
                    item.NPreco = dto.Preco;
                    item.NIdStatus = dto.StatusItemListaId ?? item.NIdStatus;
                    
                    // Se o item foi atualizado/marcado, definimos o usuário comprador
                    if (dto.StatusItemListaId != null)
                    {
                        item.NIdUsuarioComprador = usuarioLogadoId;
                    }
                }
            }

            return itensAtualizar;
        }
    }
}