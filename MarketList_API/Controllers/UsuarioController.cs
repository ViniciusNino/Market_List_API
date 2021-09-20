using System;
using System.Threading.Tasks;
using MarketList_Business;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;

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
    }
}
