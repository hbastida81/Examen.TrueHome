using Examen.Data.Connection;
using Examen.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Data
{
	public class PropertyServices : Interfaces.DataServices.IPropertyServicesDL
	{
		private readonly IConnection _connection;
		public PropertyServices(IConnection connection)
		{
			_connection = connection;
		}
		public Task<Property> Add(Property entity)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Property>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<Property> GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Property> Modify(Property entity)
		{
			throw new NotImplementedException();
		}
	}
}
