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

        public async Task<UsuarioAutenticadoVM> AutenticarUsuario(InformacaoAutenticacaoUsuarioDTO informacaoAutenticacaoUsuario)
        {
            try
            {
                using (_context)
                {
                    return await _context.Usuario
                        .Where(x => x.SSenha == informacaoAutenticacaoUsuario.Senha && x.SUsuario == informacaoAutenticacaoUsuario.Usuario)
                        .Join(_context.Unidade, us => us.NIdUnidade, un => un.Id, (us, un) =>
                        new UsuarioAutenticadoVM
                        {
                            Nome = us.SUsuario,
                            UnidadeId = us.NIdUnidade,
                            Id = us.Id,
                            PerfilId = us.NIdPerfilUsuario,
                            NomeUnidade = un.SNome
                        }).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("[UsuarioRepositury - AutenticarUsuario]", ex);
            }
        }
    }
}