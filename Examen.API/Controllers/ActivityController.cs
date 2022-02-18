using Examen.Entity;
using Examen.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace Examen.API.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IActivityServices services;

        public ActivityController(ILogger<ActivityController> logger, IActivityServices services)
        {
            _logger = logger;
            this.services = services;
        }
        /// <summary>
        /// Este método inserta nueva actividad
        /// </summary>
        /// <remarks>Este es un remark para información ampliada</remarks>
        /// <response code="200">Todo bien!</response>
        /// <response code="400">Algo Fallo</response>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Activity>> Post(Activity entity)
        {
            Activity response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    response = await this.services.Add(entity);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, errors = ex.Message });
            }
        }
    }
}
