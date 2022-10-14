using System.Threading.Tasks;
using MarketList_Data;

namespace MarketList_Repository.UnidadeDeTrabalho
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        private readonly MarketListContext _context;

        public UnidadeDeTrabalho(MarketListContext context)
        {
            _context = context;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}