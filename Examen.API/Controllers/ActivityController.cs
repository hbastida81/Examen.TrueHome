using Examen.Entity;
using Examen.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        /// EndPoint Inserta nueva actividad
        /// </summary>
        /// <remarks>Este es un remark para información ampliada</remarks>
        /// <response code="200">Todo bien!</response>
        /// <response code="400">Algo Fallo</response>
        [HttpPost]
        [Route("Add")]
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

        /// <summary>
        /// EndPoint que permite reagendar una actividad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Reschedule")]
        public async Task<ActionResult<Activity>> Reschedule(Activity entity)
        {
            Activity response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    response = await this.services.Reschedule(entity);
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, errors = ex.Message });
            }
        }

        /// <summary>
        /// EndPoint actualiza estatus de la actividad y la pone como actividad cancelada
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Cancel")]
        public async Task<ActionResult<Activity>> Cancel(Activity entity)
        {
            Activity response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    response = await this.services.Cancel(entity);
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, errors = ex.Message });
            }
        }

        /// <summary>
        /// EndPoint que retorna el listado de actividades existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Getall")]
        public async Task<ActionResult<IEnumerable<Activity>>> Get()
        {
            IEnumerable<Activity> response = null;
            try
            {
                response = await this.services.GetAll();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// EndPoint muestra listado de actividadesexistentes 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Getbyfilters")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetByFilters(Parameters parameters)
        {
            IEnumerable<Activity> response = null;
            try
            {
                response = await this.services.GetByFilters(parameters);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        /// <summary>
        /// EndPoint que permite finalizar una actividad
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //[HttpPut]
        //[Route("terminate")]
        //public async Task<ActionResult<Activity>> Terminate(Activity entity)
        //{
        //    Activity response = null;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            response = await this.services.Terminate(entity);
        //        }
        //        return Ok(response);

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { status = 500, errors = ex.Message });
        //    }
        //}
    }
}
