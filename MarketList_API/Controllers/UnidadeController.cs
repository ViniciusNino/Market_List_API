using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MarketList_API.Response;
using MarketList_Business;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnidadeController : ControllerBase
    {
        private readonly IUnidadeBusiness _unidadeBus;
        private readonly IUnidadeCadastroBusiness _unidadeCadBus;
        private readonly IMapper _mapper;

        public UnidadeController(IUnidadeCadastroBusiness unidadeCadBus, IUnidadeBusiness unidadeBus, IMapper mapper)
        {
            _unidadeCadBus = unidadeCadBus;
            _unidadeBus = unidadeBus;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Listas/{usuarioId}")]
        public async Task<IActionResult> GetUnidadesEListas(int usuarioId)
        {
            try
            {
                var response = await _unidadeBus.GetUnidadesEListas(usuarioId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Ocorreu um erro ao buscar as Unidades e lista. Tente novamente mais tarde.",
                    referencia = ex.Message,
                    stack = ex.StackTrace,
                    innerExcpetion = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                });
            }
        }

        [HttpGet]
        [Route("Unidades/{usuarioId:int}")]
        public async Task<IActionResult> GetUnidades(int usuarioId)
        {
            try
            {
                var response = await _unidadeBus.GetUnidades(usuarioId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Ocorreu um erro ao buscar as Unidades e lista. Tente novamente mais tarde.",
                    referencia = ex.Message,
                    stack = ex.StackTrace,
                    innerExcpetion = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                });
            }
        }

        [HttpPost]
        [Route("Unidades")]
        public async Task<IActionResult> SetUnidade(UnidadeCadastroDTO unidadeCadastro)
        {
            ApiResponse response = null;

            if (unidadeCadastro == null || unidadeCadastro.IdUsuario == 0 || string.IsNullOrEmpty(unidadeCadastro.Nome))
            {
                response = new ApiResponse("Credenciais inválidas", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }
            try
            {
                var result = await _unidadeCadBus.SetUnidade(unidadeCadastro);
                 response = new ApiResponse(
                    new 
                    {
                        Cadastrado = true
                    },
                    HttpStatusCode.OK
                );
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensagem = "Ocorreu um erro ao buscar as Unidades e lista. Tente novamente mais tarde.",
                    referencia = ex.Message,
                    stack = ex.StackTrace,
                    innerExcpetion = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                });
            }
        }
    }
}