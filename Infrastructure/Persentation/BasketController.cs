using Microsoft.AspNetCore.Mvc;
using Services_Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {

        [HttpGet]
        public async Task <IActionResult> GetBasketById(string id)
        {
              var result = await serviceManager.BasketService.GetBasketAsync(id);

            return Ok  (result);
        }

        [HttpPost]
        public async Task <IActionResult> UpdateBasketAsync (BasketDto basketDto)
        {
            var result = await serviceManager.BasketService.UpdateBasketAsync( basketDto);

            return Ok (result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            await serviceManager.BasketService.DeleteBasketAsync( id);
            return NoContent(); //204
        }
    }
}
