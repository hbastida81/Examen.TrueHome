using Examen.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Interfaces.DataServices
{
	public interface IActivityServicesDL : IBaseServicesDL<Activity>
	{
		Task<Activity> Exists(int property_id, DateTime schedule);
		Task<Activity> Reschedule(Activity entity);
		Task<Activity> Cancel(Activity entity);
		Task<IEnumerable<Activity>> GetByFilters(Parameters parameters);
		Task<Activity> Terminate(Activity entity);
	}
}
