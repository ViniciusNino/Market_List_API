using System;
using System.Threading.Tasks;
using AutoMapper;
using MarketList_Business;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnidadeController : ControllerBase
    {
        private readonly IUnidadeBusiness _unidadeBus;
        private readonly IMapper _mapper;

        public UnidadeController(IUnidadeBusiness unidadeBus, IMapper mapper)
        {
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
    }
}