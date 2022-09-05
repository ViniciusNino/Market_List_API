using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class ListaRepository : BaseRepository<Lista>, IListaRepository
    {
        private readonly MarketListContext _contexto;

        public ListaRepository(MarketListContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }
        public async Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId)
        {
            try
            {
                using (_contexto)
                {
                    return await _contexto.Lista.Where(x => x.NIdUnidade == unidadeId && x.BAtivo == true)
                                .Join(_contexto.Usuario, l => l.NIdUsuario, us => us.Id, (l, us) =>
                                new ListaDTO{
                                    Cadastro = l.DCadastro,
                                    Id = l.Id,
                                    Nome = l.SNome,
                                    NomeUsuario = us.SUsuario
                                }).ToListAsync();
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaRepository - GetListaPorUnidadeId] - " + ex.Message, ex);
            }
        }

        public async Task<bool> SetLista(SalvarListaDTO listaDTO)
        {
           try
           {
               using (_contexto)
               {
                    var listaBanco = MontarListaParaSalvar(listaDTO);                    
                    await _contexto.Lista.AddAsync(listaBanco);
                    var retorno = await _contexto.SaveChangesAsync();

                    var itensLista = MontarItensListaParaSalvar(listaDTO.ItensLista, listaBanco);
                    await _contexto.ItemLista.AddRangeAsync(itensLista);
                    retorno += await _contexto.SaveChangesAsync();
                    

                    return retorno > 2 ? true : false;
               }
           }
           catch (Exception ex)
           {
                throw new Exception("[ListaRepository/SetLista] - " + ex.Message, ex);
           }
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
                NIdStatusItemLista = (int)EStatusItemLista.Solicitado
            }));

            return itensListaBanco;
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

        public async Task<int> DeleteLista(int listaId) 
        {
            _contexto.Lista.Remove(_contexto.Lista.Where(x => x.Id == listaId).FirstOrDefault());
            return await _contexto.SaveChangesAsync();
        }
    }
}