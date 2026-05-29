using System;
using System.Net;
using System.Threading.Tasks;
using MarketList_API.Response;
using MarketList_Business;
using MarketList_Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessaoController : ControllerBase
    {
        private readonly ISessaoBusiness _sessaoBusiness;

        public SessaoController(ISessaoBusiness sessaoBusiness)
        {
            _sessaoBusiness = sessaoBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _sessaoBusiness.GetAllAsync();
                return Ok(new ApiResponse(result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _sessaoBusiness.FindByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse("Mercado não encontrado", HttpStatusCode.NotFound));

                return Ok(new ApiResponse(result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Sessao sessao)
        {
            if (sessao == null || string.IsNullOrEmpty(sessao.SNome))
                return BadRequest(new ApiResponse("Dados do mercado inválidos", HttpStatusCode.BadRequest));

            try
            {
                var result = await _sessaoBusiness.AddAsync(sessao);
                return Created(string.Empty, new ApiResponse(result, HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Sessao sessao)
        {
            if (sessao == null || string.IsNullOrEmpty(sessao.SNome) || id != sessao.Id)
                return BadRequest(new ApiResponse("Dados do mercado inválidos", HttpStatusCode.BadRequest));

            try
            {
                var dbSessao = await _sessaoBusiness.FindByIdAsync(id);
                if (dbSessao == null)
                    return NotFound(new ApiResponse("Mercado não encontrado", HttpStatusCode.NotFound));

                dbSessao.SNome = sessao.SNome;
                await _sessaoBusiness.UpdateAsync(dbSessao);
                return Ok(new ApiResponse(dbSessao, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dbSessao = await _sessaoBusiness.FindByIdAsync(id);
                if (dbSessao == null)
                    return NotFound(new ApiResponse("Mercado não encontrado", HttpStatusCode.NotFound));

                await _sessaoBusiness.RemoveAsync(id);
                return Ok(new ApiResponse("Mercado removido com sucesso", HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
