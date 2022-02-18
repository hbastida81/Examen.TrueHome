using Examen.Entity;
using Examen.Interfaces.DataServices;
using Examen.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Transversal
{
	public class ActivityBusiness : BaseService<Activity>, Interfaces.Services.IActivityServices
	{
		private readonly IActivityServicesDL activityServiceDL;
		private readonly IPropertyServicesDL propertyServiceDL;

		public ActivityBusiness
			(
			IActivityServicesDL activityServiceDL,
			IPropertyServicesDL propertyServiceDL
			) : base(activityServiceDL)
		{
			this.activityServiceDL = activityServiceDL;
			this.propertyServiceDL = propertyServiceDL;
		}

		public async override Task<Activity> Add(Activity entity)
		{

			//validamos que la propiedad este activa
			var property = await this.propertyServiceDL.GetById(entity.Property_Id);
			if (property == null)
				throw new Exception("Propiedad invalida");

			if (property.Property_Status.Equals("Disabled", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new Exception("Propiedad deshabilitada");
			}

			//No se pueden crear actividades en la misma fecha y hora (para la misma propiedad)
			//verificamos que no haya una actividad en la misma hora para la misma propiedad
			var activityExists = await this.activityServiceDL.Exists(entity.Property_Id, entity.Activity_Schedule);
			if (activityExists != null)
			{
				throw new Exception("No se pueden crear actividades en la misma fecha y hora!");
			}


			if (string.IsNullOrEmpty(entity.Activity_Title))
			{
				throw new Exception("Activity_Title es requerida");
			}

			if (entity.Activity_Schedule.Equals(default(DateTime)))
			{
				throw new Exception("Activity_Schedule es requerida");
			}

			var _activity = await base.Add(entity);

			var _newActivity = await base.GetById(entity.Activity_Id);

			return _newActivity;
		}
		public async Task<Activity> Cancel(Activity entity)
		{
			//verificamos que la activida exista
			var activity = await base.GetById(entity.Activity_Id);
			if (activity == null)
			{
				throw new Exception("La actividad no existe");
			}


			if (activity.Activity_Status.Equals("Cancelled"))
			{
				throw new Exception("No es posible cancelar una actividad previamente cancelada!");
			}

			await this.activityServiceDL.Cancel(entity);
			activity = await base.GetById(entity.Activity_Id);

			return activity;
		}

		public async Task<IEnumerable<Activity>> GetByFilters(Parameters parameters)
		{
			return await this.activityServiceDL.GetByFilters(parameters);
		}

		public async Task<Activity> Reschedule(Activity entity)
		{
			//verificamos que la activida exista
			var activity = await base.GetById(entity.Activity_Id);
			if (activity == null)
			{
				throw new Exception("La actividad no existe");
			}

			if (activity.Activity_Status.Equals("Cancelled"))
			{
				throw new Exception("No es posible programar la actividad porque ha sido cancelada");
			}


			//verificamos que no haya una actividad en la misma hora para la misma propiedad
			var activityExists = await this.activityServiceDL.Exists(activity.Property_Id, entity.Activity_Schedule);
			if (activityExists != null)
			{
				throw new Exception("Ya hay una actividad programada para esa hora");
			}

			await this.activityServiceDL.Reschedule(entity);
			activity = await base.GetById(entity.Activity_Id);

			return activity;
		}

		public async Task<Activity> Terminate(Activity entity)
		{
			//verificamos que la activida exista
			var activity = await base.GetById(entity.Activity_Id);
			if (activity == null)
			{
				throw new Exception("La actividad no existe");
			}


			if (activity.Activity_Status.Equals("Cancelled"))
			{
				throw new Exception("No es posible dar por terminada una actividad previamente cancelada");
			}

			await this.activityServiceDL.Terminate(entity);
			return await base.GetById(entity.Activity_Id);
		}

		public Task<int> Delete(int id) => throw new NotImplementedException();

	}
}
