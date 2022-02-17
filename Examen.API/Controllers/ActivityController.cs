using Examen.Entity;
using Examen.Entity.ActivityDto;
using Examen.Interfaces.Services;
using Examen.Tranversal.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace Examen.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IActivityServices service;

        public ActivityController(ILogger<ActivityController> logger, IActivityServices service)
        {
            _logger = logger;
            this.service = service;
        }
        /// <summary>
        /// Insertar una nueva actividad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Activity>> Post(Activity entity)
        {
            Activity response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    response = await this.service.Add(entity);
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
