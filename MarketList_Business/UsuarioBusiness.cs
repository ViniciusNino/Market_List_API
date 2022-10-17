using System;
using System.Linq;
using System.Threading.Tasks;
using MarketList_Model;
using MarketList_Repository;

namespace MarketList_Business
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public UsuarioBusiness(IUsuarioRepository usuarioRepository, IUnidadeDeTrabalho unidadeTrab) : base(usuarioRepository, unidadeTrab)
        {
            _usuarioRepository = usuarioRepository;
            _unidadeTrab = unidadeTrab;
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

        public async Task<bool> EmailExiste(string email)
        {
            var usuario = await FindAsync(us => us.SEmail == email);

            return usuario.Any();
        }
    }
}