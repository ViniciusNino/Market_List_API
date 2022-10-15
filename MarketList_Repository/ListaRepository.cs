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
        private readonly MarketListContext _context;

        public ListaRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ListaDTO>> GetListaPorUnidadeId(int unidadeId)
        {
            try
            {
                using (_context)
                {
                    return await _context.Lista.Where(x => x.NIdUnidade == unidadeId && x.BAtivo == true)
                                .Join(_context.Usuario, l => l.NIdUsuario, us => us.Id, (l, us) =>
                                new ListaDTO
                                {
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

        public async Task<Lista> SetLista(Lista lista)
        {
            try
            {
                await _context.Lista.AddAsync(lista);
                var retorno = await _context.SaveChangesAsync();

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaRepository/SetLista] - " + ex.Message, ex);
            }
        }

        public async Task<int> DeleteLista(int listaId)
        {
            _context.Lista.Remove(_context.Lista.Where(x => x.Id == listaId).FirstOrDefault());
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SalvarAgrupado(Agrupador agrupado)
        {
            try
            {
                await _context.AgrupadorListas.AddAsync(agrupado);
                await _context.SaveChangesAsync();

                return agrupado.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - SalvarAgrupado] - {ex.Message}", ex);
            }
        }

        public async Task<int> SalvarAgrupadosListas(List<ListaAgrupador> lista)
        {
            try
            {
                await _context.ListaAgrupadorListas.AddRangeAsync(lista);

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - SalvarAgrupadosListas] - {ex.Message}", ex); ;
            };
        }

        public async Task<List<ItensListasAgrupadasDTO>> GetListaAgrupadas(int usuarioId)
        {
            try
            {
                return await (from al in _context.AgrupadorListas
                              join lal in _context.ListaAgrupadorListas on al.Id equals lal.NIdAgrupadorListas
                              join il in _context.ItemLista on lal.NIdLista equals il.NIdLista
                              join i in _context.Item on il.NIdItem equals i.Id
                              where al.NIdUsuario == usuarioId
                              group new { al, i, il } by new { i.SNome, al.SDescricao } into listas
                              select new ItensListasAgrupadasDTO()
                              {
                                  IdAgrupador = listas.Min(x => x.al.Id),
                                  NomeAgrupador = listas.Key.SDescricao,
                                  NomeItem = listas.Key.SNome,
                                  UnidadeMedida = listas.Min(x => x.i.SUnidadeMedida),
                                  Quantidade = listas.Sum(x => x.il.NQuantidade),
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - GetListaAgrupadas] - {ex.Message}", ex);
            }
        }
    }
}