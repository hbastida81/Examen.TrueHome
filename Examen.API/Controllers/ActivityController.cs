using Examen.Entity.ActivityDto;
using Examen.Tranversal.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace Examen.API.Controllers
{
	[Route("api/Activity")]
    [ApiController]
    public class ActivityController : Controller
    {
        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(Response<ActivityDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response<IEnumerable<ActivityDTO>>>> GetAllSuppliersAsync()
        {
            var response = new Response<IEnumerable<ActivityDTO>>();

            //response = await _catalogsApplication.GetAllSuppliersAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(response);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized(response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
