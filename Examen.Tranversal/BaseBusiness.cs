using Examen.Interfaces.DataServices;
using Examen.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Transversal
{
	public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
	{
		//Declaración de variables globales
		private readonly IBaseServicesDL<TEntity> baseservicesDL;
		public BaseService(IBaseServicesDL<TEntity> baseservicesDL)
		{
			this.baseservicesDL = baseservicesDL;
		}

		/**
		* <summary>Método que permite ingresar una entidad</summary>
		* <param name="entity">Corresponde a la entidad que se desea agregar</param>
		*/
		public async Task<TEntity> Add(TEntity entity)
		{
			return await this.baseservicesDL.Add(entity);
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TEntity>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> Modify(TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}
