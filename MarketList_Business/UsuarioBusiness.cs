using System;
using System.Linq;
using System.Threading.Tasks;
using MarketList_API.Util;
using MarketList_Model;
using MarketList_Model.DTO;
using MarketList_Repository;
using BC = BCrypt.Net.BCrypt;

namespace MarketList_Business
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {
        private const int TempoExipacaoSenhaTemporaria = 60;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IVerificacaoTokenRepository _verificacaoTokenRepository;
        private readonly IUnidadeDeTrabalho _unidadeTrab;

        public UsuarioBusiness(IUsuarioRepository usuarioRepository, IUnidadeDeTrabalho unidadeTrab, IVerificacaoTokenRepository verificacaoTokenRepository) : base(usuarioRepository, unidadeTrab)
        {
            _usuarioRepository = usuarioRepository;
            _unidadeTrab = unidadeTrab;
            _verificacaoTokenRepository = verificacaoTokenRepository;
        }

        public async Task<bool> EmailExiste(string email)
        {
            var usuario = await FindAsync(us => us.SEmail == email);

            return usuario.Any();
        }

        public async Task<UsuarioCadastroDTO> GetUserCadastroDto(string email)
        {
            var usuario = await GetFirstOrDefaultAsync(us => us.SEmail == email);
            return new UsuarioCadastroDTO
            {
                Id = usuario.Id,
                Email = usuario.SEmail,
                Nome = usuario.SNome,
                Status = usuario.NIdStatus
            };
        }

        public async Task<UsuarioAutenticadoDTO> GetUsuarioAutenticado(LoginDTO loginDto)
        {
            var usuario = await GetFirstOrDefaultAsync
                                    (
                                        us => us.SEmail == loginDto.Email,
                                        ver => ver.Tokens.Where(x => x.NIdTipo == (int)TipoVerificacaoTokenEnum.Alterar_Senha
                                                                    && x.BAtivo == true)
                                    );

            var usuarioDto = CriarUsuarioAutenticado(usuario, loginDto.Senha);
            _verificacaoTokenRepository.UpdateRange(usuario.Tokens.Where(x => x.BAtivo == false).ToList());
            await _unidadeTrab.CommitAsync();

            return usuarioDto;
        }

        private UsuarioAutenticadoDTO CriarUsuarioAutenticado(Usuario usuario, string senha)
        {
            VerificacaoToken token = null;
            usuario.Tokens.ToList().ForEach(tk =>
            {
                tk.NoPrazo();
                if (BC.Verify(senha, tk.Token))
                {
                    token = tk;
                };

            });
            string senhaToken = null;
            bool senhaTemporariaExpirada = false;

            if (token != null)
            {
                senhaToken = token.Token;
                senhaTemporariaExpirada = !token.NoPrazo();
            }

            return new UsuarioAutenticadoDTO()
            {
                Id = usuario.Id,
                Nome = usuario.SNome,
                Email = usuario.SEmail,
                Senha = usuario.SSenha,
                StatusId = usuario.NIdStatus,
                TipoId = usuario.NIdTipo,
                SenhaToken = senhaToken,
                SenhaTemporariaExpirada = senhaTemporariaExpirada,
                EmailConfirmado = usuario.NIdStatus != (int)StatusUsuarioEnum.Aguardando_Ativacao_Email,
                SenhaTemporaria = senhaToken != null
            };
        }
    }
}