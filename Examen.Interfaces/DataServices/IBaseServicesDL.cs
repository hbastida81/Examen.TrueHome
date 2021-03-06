using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Interfaces.DataServices
{
	public interface IBaseServicesDL<TEntity> where TEntity : class
	{
		Task<TEntity> Add(TEntity entity);
		Task<int> Delete(int id);
		Task<TEntity> Modify(TEntity entity);
		Task<IEnumerable<TEntity>> GetAll();
		Task<TEntity> GetById(int id);
	}
}
