using MarketList_Data;
using MarketList_Model;

namespace MarketList_Repository
{
    public class SessaoRepository : BaseRepository<Sessao>, ISessaoRepository
    {
        private readonly MarketListContext _context;

        public SessaoRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }
    }
}
