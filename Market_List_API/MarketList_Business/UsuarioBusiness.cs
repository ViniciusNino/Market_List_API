using System;
using System.Linq;
using System.Threading.Tasks;
using MarketList_API.Util;
using MarketList_Model;
using MarketList_Model.DTO;
using MarketList_Repository;
using static MarketList_Model.VerificacaoToken;
using BC = BCrypt.Net.BCrypt;

namespace MarketList_Business
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {
        private readonly IVerificacaoTokenRepository _verificacaoTokenRepository;

        public UsuarioBusiness(IUsuarioRepository usuarioRepository, IUnidadeDeTrabalho unidadeTrab, IVerificacaoTokenRepository verificacaoTokenRepository) : base(usuarioRepository, unidadeTrab)
        {
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
                    token = tk;

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

        public async Task<UsuarioAutenticadoDTO> AuthenticateSocial(SocialLoginDTO socialLoginDto)
        {
            string email = null;
            string nome = null;

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                if (socialLoginDto.Provider.ToLower() == "google")
                {
                    var response = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={socialLoginDto.Token}");
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Token do Google inválido ou expirado.");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var googleUser = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
                    email = googleUser.email;
                    nome = googleUser.name;
                }
                else if (socialLoginDto.Provider.ToLower() == "facebook")
                {
                    var response = await httpClient.GetAsync($"https://graph.facebook.com/me?fields=id,name,email&access_token={socialLoginDto.Token}");
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Token do Facebook inválido ou expirado.");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var facebookUser = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(content);
                    email = facebookUser.email;
                    nome = facebookUser.name;
                }
                else
                {
                    throw new Exception("Provedor de login social inválido.");
                }
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Não foi possível obter o e-mail do usuário do provedor social.");
            }

            var usuario = await GetFirstOrDefaultAsync(us => us.SEmail == email);

            if (usuario == null)
            {
                var novaSenhaHash = BC.HashPassword(Guid.NewGuid().ToString());
                
                usuario = new Usuario(novaSenhaHash, email, (int)PerfitlUsuarioEnum.Administrador, (int)StatusUsuarioEnum.Ativo)
                {
                    SNome = nome,
                    DCadastro = DateTime.Now
                };

                await AddAsync(usuario);
            }
            else if (usuario.NIdStatus == (int)StatusUsuarioEnum.Aguardando_Ativacao_Email)
            {
                usuario.ConfirmarEmail();
                await UpdateAsync(usuario);
            }

            return new UsuarioAutenticadoDTO()
            {
                Id = usuario.Id,
                Nome = usuario.SNome,
                Email = usuario.SEmail,
                Senha = usuario.SSenha,
                StatusId = usuario.NIdStatus,
                TipoId = usuario.NIdTipo,
                SenhaToken = null,
                SenhaTemporariaExpirada = false,
                EmailConfirmado = true,
                SenhaTemporaria = false
            };
        }
    }
}