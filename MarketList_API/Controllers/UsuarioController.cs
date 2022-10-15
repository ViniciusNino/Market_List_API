using System.Net;
using System;
using System.Threading.Tasks;
using MarketList_Business;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using MarketList_Model.DTO;
using MarketList_API.Response;

namespace MarketList_API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioBusiness _usuarioBusiness;

        public UsuarioController(IUsuarioBusiness usuarioBusiness)
        {
            _usuarioBusiness = usuarioBusiness;
        }

        [HttpGet]
        [Route("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(string usuario, string senha)
        {
            var informacaoAutenticacaoUsuario = new InformacaoAutenticacaoUsuarioDTO
            {
                Senha = senha,
                Usuario = usuario
            };
            try
            {
                var usuarioAutenticado = await _usuarioBusiness.AutenticarUsuario(informacaoAutenticacaoUsuario);
                return Ok(usuarioAutenticado);
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
        ///       "sEmail": "john.doe@prosprapp.com",      
        ///       "sSenha": "asdzxc",
        ///       "sNome": "recruiter1" 
        ///     }
        ///     
        /// Sample response:
        /// 
        ///     Http status Created
        ///     
        /// </remarks>
        /// <response code="201">User registered</response>
        /// <response code="400">Invalid parameters</response>
        /// <response code="409">E-mail address is already registered</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("Registrar")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Conflict)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CadastrarUsuario(UsuarioCadastroDTO usuario)
        {
            ApiResponse response = null;

            if (usuario == null || string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrEmpty(usuario.Nome))
            {
                response = new ApiResponse("Parâmetros inválidos", HttpStatusCode.BadRequest);
                BadRequest(response);
            }



            try
            {
                Util.SendEmail.Send(usuario.Email, "teste");

                return Ok("Ok!");
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

        [HttpGet]
        [Route("Validar/Email")]
        public async Task<IActionResult> ValidarEmail(string token)
        {
            try
            {
                return Ok(token);
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


    }
}
