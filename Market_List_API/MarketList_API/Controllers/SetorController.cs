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
    public class SetorController : ControllerBase
    {
        private readonly ISetorBusiness _setorBusiness;

        public SetorController(ISetorBusiness setorBusiness)
        {
            _setorBusiness = setorBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _setorBusiness.GetAllAsync();
                return Ok(new ApiResponse(result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpGet("Mercado/{sessaoId:int}")]
        public async Task<IActionResult> GetByMercado(int sessaoId)
        {
            try
            {
                var result = await _setorBusiness.FindAsync(s => s.NIdSessao == sessaoId);
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
                var result = await _setorBusiness.FindByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponse("Setor não encontrado", HttpStatusCode.NotFound));

                return Ok(new ApiResponse(result, HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Setor setor)
        {
            if (setor == null || !setor.IsValid())
                return BadRequest(new ApiResponse("Dados do setor inválidos", HttpStatusCode.BadRequest));

            try
            {
                var result = await _setorBusiness.AddAsync(setor);
                return Created(string.Empty, new ApiResponse(result, HttpStatusCode.Created));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Setor setor)
        {
            if (setor == null || !setor.IsValid() || id != setor.Id)
                return BadRequest(new ApiResponse("Dados do setor inválidos", HttpStatusCode.BadRequest));

            try
            {
                var dbSetor = await _setorBusiness.FindByIdAsync(id);
                if (dbSetor == null)
                    return NotFound(new ApiResponse("Setor não encontrado", HttpStatusCode.NotFound));

                dbSetor.SNome = setor.SNome;
                dbSetor.NIdSessao = setor.NIdSessao;
                await _setorBusiness.UpdateAsync(dbSetor);
                return Ok(new ApiResponse(dbSetor, HttpStatusCode.OK));
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
                var dbSetor = await _setorBusiness.FindByIdAsync(id);
                if (dbSetor == null)
                    return NotFound(new ApiResponse("Setor não encontrado", HttpStatusCode.NotFound));

                await _setorBusiness.RemoveAsync(id);
                return Ok(new ApiResponse("Setor removido com sucesso", HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message, HttpStatusCode.BadRequest));
            }
        }
    }
}
