using System.Threading.Tasks;
using MarketList_Model;

namespace MarketList_Business
{
    public interface IUsuarioBusiness : IBaseBusiness<Usuario>
    { 
        Task<UsuarioAutenticadoViewModel> AutenticarUsuario(InformacaoAutenticacaoUsuarioDTO informacaoAutenticacaoUsuario);
     }
}