using System;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {
        private readonly IUsuarioRepository _usuarioRepository;
        
        public UsuarioBusiness(IUsuarioRepository usuarioRepository) : base (usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioAutenticadoVM> AutenticarUsuario(InformacaoAutenticacaoUsuarioDTO InformacaoAutenticacaoUsuario)
        {
            try
            {
                if (string.IsNullOrEmpty(InformacaoAutenticacaoUsuario.Senha) && string.IsNullOrEmpty(InformacaoAutenticacaoUsuario.Usuario))
                    return null;

                return await _usuarioRepository.AutenticarUsuario(InformacaoAutenticacaoUsuario);
                
            }
            catch (Exception ex)
            {
                throw new Exception($"[UsuarioBusiness - AutenticarUsuario] - {ex.Message}", ex);
            }
        }
    }
}