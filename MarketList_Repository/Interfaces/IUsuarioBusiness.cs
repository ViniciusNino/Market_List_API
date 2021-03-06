using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<UsuarioAutenticadoVM> AutenticarUsuario(InformacaoAutenticacaoUsuarioDTO InformacaoAutenticacaoUsuario);
    }
}