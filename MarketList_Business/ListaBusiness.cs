using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketList_Data;
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
            var listaBanco = MontarListaParaSalvar(lista);
            listaBanco = await _listaRepository.SetLista(listaBanco);
            var itensLista = MontarItensListaParaSalvar(lista.ItensLista, listaBanco);

            return await _itemListaRep.SalvarItensLista(itensLista) > 2;
        }

        private Lista MontarListaParaSalvar(SalvarListaDTO lista)
        {
            return new Lista
            {
                BAtivo = true,
                DCadastro = DateTime.Now,
                NIdUnidade = lista.UnidadeId,
                NIdUsuario = lista.UsuarioId,
                SNome = lista.Nome
            };
        }

        private List<ItemLista> MontarItensListaParaSalvar(List<ItemListaDTO> itensLista, Lista lista)
        {
            var itensListaBanco = new List<ItemLista>();
            itensLista.ForEach(x => itensListaBanco.Add(new ItemLista
            {
                BAtivo = true,
                DCadastro = DateTime.Now,
                NIdLista = lista.Id,
                NIdItem = x.ItemId,
                NQuantidade = x.Quantidade,
                NIdUsuarioSolicitante = x.UsuarioLogadoId,
                NIdStatus = (int)EStatusItemLista.Solicitado
            }));

            return itensListaBanco;
        }

        public async Task<bool> DeleteLista(int listaId)
        {
            await _itemListaRep.DeletarItensLista(listaId);
            var quantidadeListaDeletada = await _listaRepository.DeleteLista(listaId);
            return quantidadeListaDeletada > 0;
        }

        public async Task<bool> SetAgrupado(AgrupadorListasDTO agrupadoDto)
        {
            var agrupadoId = await _listaRepository.SalvarAgrupado(CriarAgrupado(agrupadoDto));
            return await _listaRepository.SalvarAgrupadosListas(CriarAgrupadosListas(agrupadoId, agrupadoDto.ListaIds)) > 0;
        }

        private AgrupadorListas CriarAgrupado(AgrupadorListasDTO agrupado)
        {
            return new()
            {
                DCadastro = DateTime.Now,
                NIdStatus = (int)StatusAgrupadoEnum.Ativo,
                NIdUsuario = agrupado.UsuarioId,
                // SDescricao = agrupado.Descricao
            };
        }

        private List<ListaAgrupadorListas> CriarAgrupadosListas(int agrupadoId, int[] agrupadoDto)
        {
            var lista = new List<ListaAgrupadorListas>();
            for (int i = 0; i < agrupadoDto.Length; i++)
            {
                lista.Add(new ListaAgrupadorListas()
                {
                    NIdAgrupadorListas = agrupadoId,
                    NIdLista = agrupadoDto[i]
                });
            }

            return lista;
        }
    }
}