using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class ItemListaRepository : BaseRepository<ItemLista>, IItemListaRepository
    {
        private readonly MarketListContext _context;

        public ItemListaRepository(MarketListContext context)
        {
            _context = context;
        }

        public async Task<List<ItemListaDTO>> GetGetItensListaPorListaId(int listaId)
        {
            try
            {
                using (_context)
                {
                    return await _context.ItemLista.Where(x => x.NIdLista == listaId)
                    .Join(_context.Item, il => il.NIdItem, i => i.Id, (il, i) =>
                    new ItemListaDTO
                    {
                        Nome = i.SNome,
                        Quantidade = il.NQuantidade,
                        UnidadeMedida = i.SUnidadeMedida,
                        Id = il.Id,
                        ListaId = il.NIdLista,
                        ItemId = il.NIdItem
                    }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - GetGetItensListaPorListaId] - {ex.Message}", ex);
            }
        }

        public async Task<List<ItemLista>> ListarItensListaAtualizar(List<int?> idsAtualizar)
        {
            try
            {
                return await _context.ItemLista.Where(x => idsAtualizar.Contains(x.Id)).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - ListarItensListaAtualizar] - {ex.Message}", ex);
            }
        }

        public async Task<int> AtualizarItensLista(List<ItemLista> itensAtualizar, List<int?> idsExcluir)
        {
            try
            {
                var itensAdd = itensAtualizar.Where(x => x.Id == 0).ToList();
                var itensAtt = itensAtualizar.Where(x => x.Id != 0).ToList();

                await _context.AddRangeAsync(itensAdd);
                _context.UpdateRange(itensAtt);
                _context.ItemLista.RemoveRange(_context.ItemLista.Where(x => idsExcluir.Contains(x.Id)));

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - AtualizarItensLista] - {ex.Message}", ex);
            }
        }

        public async Task<int> SalvarItensLista(List<ItemLista> itensLista)
        {
            try
            {
                using (_context)
                {
                    await _context.ItemLista.AddRangeAsync(itensLista);

                    return await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[ListaRepository/SetLista] - " + ex.Message, ex);
            }
        }

        public async Task<int> DeletarItensLista(int listaId)
        {
            try
            {
                _context.RemoveRange(_context.ItemLista.Where(x => x.NIdLista == listaId));
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception($"[ItemListaRepository - DeletarItensLista] - {ex.Message}", ex); ;
            }
        }
    }
}