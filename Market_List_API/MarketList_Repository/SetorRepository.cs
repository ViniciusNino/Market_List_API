using MarketList_Data;
using MarketList_Model;

namespace MarketList_Repository
{
    public class SetorRepository : BaseRepository<Setor>, ISetorRepository
    {
        private readonly MarketListContext _context;

        public SetorRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }
    }
}
