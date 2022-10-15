using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public ListaBusiness(IListaRepository listaReposytory, IItemListaRepository itemListaRep, IUnidadeDeTrabalho unidadeTrab) : base(listaReposytory, unidadeTrab)
        {
            _listaRepository = listaReposytory;
            _itemListaRep = itemListaRep;
            _unidadeTrab = unidadeTrab;
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
                NIdStatus = (int)StatusItemListaEnum.Solicitado
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

        private Agrupador CriarAgrupado(AgrupadorListasDTO agrupado)
        {
            return new()
            {
                DCadastro = DateTime.Now,
                NIdStatus = (int)StatusAgrupadoEnum.Ativo,
                NIdUsuario = agrupado.UsuarioId,
                SDescricao = agrupado.Descricao
            };
        }

        private List<ListaAgrupador> CriarAgrupadosListas(int agrupadoId, List<int> agrupadoDto)
        {
            return agrupadoDto.Select(x => new ListaAgrupador
            {
                NIdAgrupadorListas = agrupadoId,
                NIdLista = x
            }).ToList();
        }

        public async Task<List<ListasAgrupadasVM>> GetListaAgrupadas(int usuarioId)
        {
            var itensListasAgrupadas = await _listaRepository.GetListaAgrupadas(usuarioId);

            return itensListasAgrupadas
                    .GroupBy(x => new { x.IdAgrupador, x.NomeAgrupador })
                    .Select(x => new ListasAgrupadasVM
                    {
                        Id = x.Key.IdAgrupador,
                        Nome = x.Key.NomeAgrupador,
                        Itens = itensListasAgrupadas
                                .Where(y => y.IdAgrupador == x.Key.IdAgrupador)
                                .Select(y => new ItensListasAgrupadasVM()
                                {
                                    Nome = y.NomeItem,
                                    Quantidade = y.Quantidade,
                                    UnidadeMedida = y.UnidadeMedida
                                }).ToList()
                    }).ToList();
        }
    }
}