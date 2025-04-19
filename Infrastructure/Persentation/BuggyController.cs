using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet ("NotFound")]
        public IActionResult GetNotFoundResult ()
        {
            return NotFound(); // 404
        }

        [HttpGet("ServerError")]
        public IActionResult GetServerErrorRequest()
        {
            throw new Exception();
            return Ok(); 
        }

        [HttpGet("badRequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(); // 400
        }
        [HttpGet("badRequest/[id]")]
        public IActionResult GetBadRequest(int id) // validation error
        {
            return BadRequest(); // 400
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorizedRequest()
        {
            return Unauthorized(); // 401
        }
    }
}
