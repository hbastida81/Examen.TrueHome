using Examen.Data.Connection;
using Examen.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Examen.Data
{
	public class PropertyServices : Interfaces.DataServices.IPropertyServicesDL
	{
		private readonly IConnection _connection;
		public PropertyServices(IConnection connection)
		{
			_connection = connection;
		}
		public async Task<Property> Add(Property entity)
		{
			using IDbConnection db = _connection.GetConnection;
			string query = $"insert into public.property values ({entity.Property_Id},'{entity.Property_Title}','{entity.Property_Address}','{entity.Property_Description}','{entity.Property_Created_at.ToString("yyyy-MM-dd HH:mm:ss")}','{entity.Property_Updated_at.ToString("yyyy-MM-dd HH:mm:ss")}','{entity.Property_Disabled_at.ToString("yyyy-MM-dd HH:mm:ss")}','{entity.Property_Status}');";


			if (db.State == ConnectionState.Closed)
				db.Open();

			using (var tran = db.BeginTransaction())
			{
				try
				{
					var result = await db.ExecuteScalarAsync<int>(query, null, commandType: CommandType.Text, transaction: tran);
					tran.Commit();
					return new Property { Property_Id = result };
				}
				catch (Npgsql.NpgsqlException ex)
				{
					tran.Rollback();
					throw new Exception(ex.Message);
				}
			}
		}

		public Task<int> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Property>> GetAll()
		{
			throw new NotImplementedException();
		}

		public async Task<Property> GetById(int id)
		{
			string query = @$"SELECT 
                                tp.property_id,
                                tp.property_title,
                                tp.property_address,
                                tp.property_description,
                                tp.property_updated_at,
                                tp.property_disabled_at,
                                tp.property_status
                            FROM property tp WHERE tp.property_id = {id};";

			Property result = null;
			using (IDbConnection db = _connection.GetConnection)
			{
				if (db.State == ConnectionState.Closed)
					db.Open();

				result = await db.QueryFirstOrDefaultAsync<Property>(query, null, commandType: CommandType.Text);
				return result;
			}
		}

		public Task<Property> Modify(Property entity)
		{
			throw new NotImplementedException();
		}
	}
}
