using MarketList_Data;
using MarketList_Model;

namespace MarketList_Repository
{
    public class VerificacaoTokenRepository : BaseRepository<VerificacaoToken>, IVerificacaoTokenRepository
    {
        private readonly MarketListContext _context;

        public VerificacaoTokenRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }
    }
}