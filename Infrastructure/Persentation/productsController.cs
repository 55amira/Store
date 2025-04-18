using Microsoft.AspNetCore.Mvc;
using Services_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation
{
    // Api Controller 

    [ApiController]

    [Route(template: "api/[controller]")]

    public class ProductsController(IServiceManager serviceManager) : ControllerBase

    {

        [HttpGet] // GET: /api/Products 

        public async Task<IActionResult> GetAllProducts(int? BrandId , int? TypeId, string? sort)
        {
            var result = await serviceManager.productService.GetAllProductsAsync(BrandId, TypeId,sort);

            if (result is null) return BadRequest(); // 400 
            return Ok(result); // 200 


        }


        [HttpGet(template: "{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {

            var result = await serviceManager.productService.GetProductByIdAsync(id);


            if (result is null) return NotFound(); // 404 

            return Ok(result);

        }

        [HttpGet("Brands")]

        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.productService.GetAllBrandsAsync();

            if (result is null) return BadRequest(); // 400 
            return Ok(result); // 200 

        }
        //byId

        [HttpGet("Types")]

        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.productService.GetAlltypesAsync();

            if (result is null) return BadRequest(); // 400 
            return Ok(result); // 200 

        }
    }
}
