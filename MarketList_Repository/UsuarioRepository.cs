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
                        .Join(_context.UsuarioUnidade, us => us.Id, usun => usun.NIdUsuario, (us, usun) => new { us, usun })
                        .Join(_context.Unidade, usun => usun.usun.NIdUnidade, un => un.Id, (usun, un) => new { usun, un })
                        .Where(x => x.usun.us.SSenha == informacaoAutenticacaoUsuario.Senha && x.usun.us.SUsuario == informacaoAutenticacaoUsuario.Usuario)
                        .Select(usuario => new UsuarioAutenticadoVM
                        {
                            Nome = usuario.usun.us.SUsuario,
                            UnidadeId = usuario.usun.usun.NIdUnidade,
                            Id = usuario.usun.us.Id,
                            TipoId = usuario.usun.us.NIdTipo,
                            NomeUnidade = usuario.un.SNome
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