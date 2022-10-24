using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System;
using System.Threading.Tasks;
using MarketList_Business;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using MarketList_Model.DTO;
using MarketList_API.Response;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;
using MarketList_API.Util;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace MarketList_API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioBusiness _usuarioBusiness;
        private readonly IVerificacaoTokenBusiness _verificacaoTokenBusiness;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioBusiness usuarioBusiness, IVerificacaoTokenBusiness verificacaoTokenBusiness, IMapper mapper)
        {
            _usuarioBusiness = usuarioBusiness;
            _verificacaoTokenBusiness = verificacaoTokenBusiness;
            _mapper = mapper;
        }

        /// <summary>
        ///     Autenticar usuário no sistema
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST Autenticar
        ///     {        
        ///       "Email": "john.doe@prosprapp.com",      
        ///       "Senha": "asdzxc"     
        ///     }
        ///
        /// Sample response:
        ///     
        ///     {
        ///       "response":
        ///       {
        ///         "accessToken": "dhjashdufghfjsdfbhsjdfb jb shvhvhfbsdjafnjdbfjasdbfaskbf",      
        ///         "user":
        ///         {
        ///             "Id": 2,
        ///             "Nome": "Vinícius",
        ///             "Status": 2,
        ///             "Tipo": 2
        ///         }
        ///       },
        ///       "statusResult": 200
        ///     }
        /// </remarks>
        /// <response code="200">Usuário autenticado</response>
        /// <response code="400">Credenciais inválidas</response>
        /// <response code="403">Endereço de e-mail ainda não confirmado</response>
        /// <response code="410">Senha temporária expirada</response>
        [HttpPost]
        [Route("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(LoginDTO loginDto)
        {
            ApiResponse response = null;

            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Senha))
            {
                response = new ApiResponse("Credenciais inválidas", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            var usuario = await _usuarioBusiness.GetUsuarioAutenticado(loginDto);

            if (usuario.SenhaTemporariaExpirada)
            {
                response = new ApiResponse("Senha temporária expirada. Solicite uma nova senha.", HttpStatusCode.Gone);
                return BadRequest(response);
            }

            if (usuario == null || (!Token.ValidarSenha(loginDto.Senha, usuario.Senha) && !Token.ValidarSenha(loginDto.Senha, usuario.SenhaToken)))
            {
                response = new ApiResponse("Credenciais inválidas", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            if (!usuario.EmailConfirmado)
            {
                response = new ApiResponse("Endereço de e-mail ainda não confirmado.", HttpStatusCode.Forbidden);
                return StatusCode(StatusCodes.Status403Forbidden, response);
            }

            try
            {
                var token = Token.GerarJWTToken(usuario);
                response = new ApiResponse(
                    new
                    {
                        TokenAcesso = new JwtSecurityTokenHandler().WriteToken(token),
                        Usuario = _mapper.Map<UsuarioAutenticadoVM>(usuario)
                    },
                    HttpStatusCode.OK
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        ///     Registrar usuário no aplicativo
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST Usuario/Registrar
        ///     {        
        ///       "Email": "john.doe@prosprapp.com",      
        ///       "Senha": "asdzxc",
        ///       "Nome": "recruiter1" 
        ///     }
        ///     
        /// Sample response:
        /// 
        ///     Http status Created
        ///     
        /// </remarks>
        /// <response code="201">Usuário registrado</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="409">Endereço de email já está registrado</response>
        /// <response code="500">Erro do Servidor Interno</response>
        [HttpPost]
        [Route("Registrar")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CadastrarUsuario(UsuarioCadastroDTO usuarioDto)
        {
            ApiResponse response = null;

            if (usuarioDto == null || string.IsNullOrEmpty(usuarioDto.Email) || string.IsNullOrEmpty(usuarioDto.Senha) || string.IsNullOrEmpty(usuarioDto.Nome))
            {
                response = new ApiResponse("Parâmetros inválidos!", HttpStatusCode.BadRequest);
                BadRequest(response);
            }

            var emailJaCadastrado = await _usuarioBusiness.EmailExiste(usuarioDto.Email);

            if (emailJaCadastrado)
            {
                response = new ApiResponse("E-mail já cadastrado!", HttpStatusCode.Conflict);
                return StatusCode(StatusCodes.Status409Conflict, response);
            }

            try
            {
                usuarioDto.Senha = BC.HashPassword(usuarioDto.Senha);

                var novoUsuario = new Usuario(usuarioDto.Nome, usuarioDto.Senha, usuarioDto.Email);

                var usuario = await _usuarioBusiness.AddAsync(novoUsuario);

                if (usuario == null)
                {
                    response = new ApiResponse("Parâmetros inválidos!", HttpStatusCode.BadRequest);
                    return BadRequest(response);
                }

                await _verificacaoTokenBusiness.SetVerificacaoToken(usuarioDto, (int)TipoVerificacaoTokenEnum.Ativacao_Email);
                response = new ApiResponse("Usário cadastrado!", HttpStatusCode.Created);

                return Created(string.Empty, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Ocorreu um erro ao cadastrar seu usuário. Tente novamente mais tarde.",
                    referencia = ex.Message,
                    stack = ex.StackTrace,
                    innerExcpetion = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                });
            }
        }

        /// <summary>
        /// Enviar um novo token de verificação de e-mail para o e-mail do usuário
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST Usuario/Validar/Email
        ///     {
        ///         "email": "joe.doe@vambora.com"
        ///     }
        ///     
        /// Sample response:
        /// 
        ///     Http status Ok
        ///     
        /// </remarks>
        /// <response code="200">Token de e-mail de verificação enviado com sucesso</response>
        /// <response code="400">O endereço de e-mail é obrigatório ou o token de verificação não foi enviado</response>
        /// <response code="500">Erro do Servidor Interno</response>
        [HttpPost]
        [Route("Reenviar/Email/Validacao")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> EnviarEmailToken(EnvioEmailValidacaoTokenDTO envioEmailDto)
        {
            ApiResponse response = null;

            if (string.IsNullOrEmpty(envioEmailDto.Email))
            {
                response = new ApiResponse("E-mail necessário", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            var usuario = await _usuarioBusiness.GetUserCadastroDto(envioEmailDto.Email);

            if (usuario == null)
            {
                response = new ApiResponse("Email não cadastrado. Crie seu registro e tente novamente.", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            try
            {
                await _verificacaoTokenBusiness.SetVerificacaoToken(usuario, (int)TipoVerificacaoTokenEnum.Ativacao_Email);
                response = new ApiResponse("Token de e-mail de verificação enviado com sucesso!", HttpStatusCode.OK);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(ex?.Message, HttpStatusCode.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Validate the user's verification email token
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST Reenviar/Email/Validacao/27e897b2-9e01-4580-ed7f-08d9a8966c72
        ///     
        /// Sample response:
        /// 
        ///     Http status Ok
        ///     
        /// </remarks>
        /// <response code="200">Email validado</response>
        /// <response code="400">O token de e-mail de verificação é obrigatório</response>
        /// <response code="404">Token de e-mail de verificação não encontrado</response>
        /// <response code="409">O token de e-mail de verificação já foi confirmado</response>
        /// <response code="410">O token de e-mail de verificação expirou</response>
        /// <response code="500">Erro do Servidor Interno</response>
        [HttpGet]
        [Route("Validar/Email")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.Gone)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ValidarEmail(string token)
        {
            ApiResponse response = null;

            if (string.IsNullOrEmpty(token))
            {
                response = new ApiResponse("Verification email token is required", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            try
            {
                var bytesTextoToken = Encoding.UTF8.GetBytes(token);
                token = Convert.ToBase64String(bytesTextoToken);
                var tokenDB = await _verificacaoTokenBusiness.GetFirstOrDefaultAsync(ver => ver.Token == token, item => item.Usuario);

                if (tokenDB == null)
                {
                    response = new ApiResponse("Verificação email token não encontrado.", HttpStatusCode.NotFound);
                    return NotFound(response);
                }

                var usuario = tokenDB.Usuario;

                if (usuario.NIdStatus == (int)StatusUsuarioEnum.Ativo)
                {
                    response = new ApiResponse("O endereço de e-mail já foi confirmado.", HttpStatusCode.Conflict);
                    return StatusCode(StatusCodes.Status409Conflict, response);
                }

                var tokenValido = _verificacaoTokenBusiness.TokenValido(tokenDB);

                if (!tokenValido)
                {
                    response = new ApiResponse("O token de verificação expirou.", HttpStatusCode.Gone);
                    return StatusCode(StatusCodes.Status410Gone, response);
                }

                usuario.ConfirmarEmail();
                await _usuarioBusiness.UpdateAsync(usuario);

                response = new ApiResponse("Email validado!", HttpStatusCode.OK);


                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Ocorreu um erro ao validar email. Tente novamente mais tarde.",
                    referencia = ex.Message,
                    stack = ex.StackTrace,
                    innerExcpetion = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                });
            }
        }

        /// <summary>
        /// Enviar uma senha temporária para o e-mail do usuário
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST Enviar/Email/Senha
        ///     {
        ///         "Email": "joe.doe@vambora.com"
        ///     }
        ///     
        /// Sample response:
        /// 
        ///     Http status Ok
        ///     
        /// </remarks>
        /// <response code="200">Token de senha de verificação enviado com sucesso</response>
        /// <response code="400">O endereço de e-mail é obrigatório ou o token de senha de verificação não pôde ser enviado</response>
        /// <response code="500">Erro do Servidor Interno</response>
        [HttpPost]
        [Route("Enviar/Email/Senha")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SendEmailComSenhaTemporaria(EnvioEmailValidacaoTokenDTO envioEmailDto)
        {
            ApiResponse response = null;

            if (string.IsNullOrEmpty(envioEmailDto.Email))
            {
                response = new ApiResponse("E-mail necessário", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            var usuario = await _usuarioBusiness.GetUserCadastroDto(envioEmailDto.Email);

            if (usuario == null)
            {
                response = new ApiResponse("Email não cadastrado. Crie seu registro e tente novamente.", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            if (usuario.Status == (int)StatusUsuarioEnum.Aguardando_Ativacao_Email)
            {
                response = new ApiResponse("Email não confirmado. Entre em sua caixa de emai e confirme o mesmo.", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            try
            {
                await _verificacaoTokenBusiness.SetVerificacaoToken(usuario, (int)TipoVerificacaoTokenEnum.Alterar_Senha);
                response = new ApiResponse("Token de e-mail de verificação enviado com sucesso!", HttpStatusCode.OK);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(ex?.Message, HttpStatusCode.InternalServerError);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
