using Examen.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Interfaces.Services
{
    public interface IActivityServices : IBaseService<Activity>
    {
        Task<Activity> Reschedule(Activity entity);
        Task<Activity> Cancel(Activity entity);
        Task<IEnumerable<Activity>> GetByFilters(Parameters parameters);
        Task<Activity> Terminate(Activity entity);

    }
}
