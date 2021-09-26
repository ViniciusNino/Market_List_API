using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        private readonly MarketListContext _context;

        public ItemRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ItemViewModel>> GetItemPorUnidade(int unidadeId)
        {
            try
            {
                using (_context)
                {
                    return await _context.Item.Where(x => x.NIdUnidade == unidadeId).Select(item => 
                        new ItemViewModel
                        { 
                            Id = item.Id,
                            Nome = item.SNome,
                            UnidadeMedida = item.SUnidadeMedida
                        }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[ItemRepository - GetItemPorUnidade] - " + ex.Message, ex);
            }
        }
    }
}