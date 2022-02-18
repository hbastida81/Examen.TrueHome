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
		* <summary>Método que permite agreegar</summary>
		* <param name="entity">Corresponde a la entidad que se desea agregar</param>
		*/
		public virtual async Task<TEntity> Add(TEntity entity)
		{
			return await this.baseservicesDL.Add(entity);
		}
		/**
         * <summary>Método que permite eliminar</summary>
         * <param name="id">Corresponde al identificador de la entidad que se va a eliminar</param>
         */
		public virtual async Task Delete(int id)
		{
			await this.baseservicesDL.Delete(id);
		}
		/**
		* <summary>Método que obtiene todos los registros</summary>
		*/
		public virtual async Task<IEnumerable<TEntity>> GetAll()
		{
			return await this.baseservicesDL.GetAll();
		}
		/**
         * <summary>Método que permite obtener la información correspondiente a la entidad solicitada</summary>
         * <param name="id">Corresponde al identificador de la entidad que se desea obtener</param>
         * <returns>La información de la correspondiente ala  entidad</returns>
         */
		public virtual async Task<TEntity> GetById(int id)
		{
			return await this.baseservicesDL.GetById(id);
		}
		/**
         * <summary>Método que permite actualizar la información una entidad</summary>
         * <param name="entity">Corresponde a la entidad que se desea modificar</param>
         */
		public virtual async Task<TEntity> Modify(TEntity entity)
		{
			return await this.baseservicesDL.Modify(entity);
		}
	}
}
