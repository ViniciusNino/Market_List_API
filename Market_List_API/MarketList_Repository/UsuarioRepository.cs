using System;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Data;
using MarketList_Model;
using Microsoft.EntityFrameworkCore;

namespace MarketList_Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly MarketListContext _context;

        public UsuarioRepository(MarketListContext context) : base(context)
        {
            _context = context;
        }
    }
}