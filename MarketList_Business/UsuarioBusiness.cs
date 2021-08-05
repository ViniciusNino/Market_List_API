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

        public async Task<UsuarioAutenticadoViewModel> AutenticarUsuario(InformacaoAutenticacaoUsuarioDTO InformacaoAutenticacaoUsuario)
        {
            if (string.IsNullOrEmpty(InformacaoAutenticacaoUsuario.SSenha) && string.IsNullOrEmpty(InformacaoAutenticacaoUsuario.SUsuario))
                return null;

            return await _usuarioRepository.AutenticarUsuario(InformacaoAutenticacaoUsuario);
        }
    }
}