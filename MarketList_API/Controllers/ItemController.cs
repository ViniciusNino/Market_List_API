using System;
using System.Threading.Tasks;
using MarketList_Business;
using Microsoft.AspNetCore.Mvc;

namespace MarketList_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemBusiness _itemBusiness;

        public ItemController(IItemBusiness itemBusiness)
        {
            _itemBusiness = itemBusiness;
        }

        [HttpGet]
        [Route("GetUnidade")]
        public async Task<IActionResult> GetItemPorUnidade(int unidadeId)
        {
            try
            {
                var listaItem = await _itemBusiness.GetItemPorUnidade(unidadeId);
                return Ok(listaItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}