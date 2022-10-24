using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Model.DTO;

namespace MarketList_Business
{
    public interface IUsuarioBusiness : IBaseBusiness<Usuario>
    {
        Task<bool> EmailExiste(string email);
        Task<UsuarioCadastroDTO> GetUserCadastroDto(string email);
        Task<UsuarioAutenticadoDTO> GetUsuarioAutenticado(LoginDTO loginDto);
    }
}