using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persentation.Attributes;
using Services_Abstractions;
using Shared;
using Shared.ErrorsModels;
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
        [ProducesResponseType/*<PaginationReponse<ProductResultDto>>*/(StatusCodes.Status200OK , Type = typeof(PaginationReponse<ProductResultDto>))]    
        [ProducesResponseType/*<PaginationReponse<ProductResultDto>>*/(StatusCodes.Status500InternalServerError , Type = typeof(ErrorDetails))]    
        [ProducesResponseType/*<PaginationReponse<ProductResultDto>>*/(StatusCodes.Status400BadRequest , Type = typeof(ErrorDetails))]
        [Cache (100)]
        public async Task<ActionResult<PaginationReponse<ProductResultDto>>> GetAllProducts([FromQuery]ProductSpecificationParamters specParams)
        {
            var result = await serviceManager.productService.GetAllProductsAsync(specParams);

            //if (result is null) return BadRequest(); // 400 
            return Ok(result); // 200 


        }


        [HttpGet(template: "{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
        {

            var result = await serviceManager.productService.GetProductByIdAsync(id);


            if (result is null) return NotFound(); // 404 

            return Ok(result);

        }

        [HttpGet("Brands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandResultDto>> GetAllBrands()
        {
            var result = await serviceManager.productService.GetAllBrandsAsync();

            if (result is null) return BadRequest(); // 400 
            return Ok(result); // 200 

        }
        //byId

        [HttpGet("Types")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await serviceManager.productService.GetAlltypesAsync();

            if (result is null) return BadRequest(); // 400 
            return Ok(result); // 200 

        }
    }
}
