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
                        sNome = i.SNome,
                        nQuantidade = il.NQuantidade,
                        sUnidadeMedida = il.SUnidadeMedida
                    }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"[ItemListaRepository - GetGetItensListaPorListaId] - {ex.Message}", ex);
            }
        }
    }
}