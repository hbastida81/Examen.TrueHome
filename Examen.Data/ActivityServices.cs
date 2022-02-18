using Examen.Data.Connection;
using Examen.Entity;
using Examen.Interfaces.DataServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Examen.Data
{
	public class ActivityServices : IActivityServicesDL
	{
		private readonly IConnection _connection;
		public ActivityServices(IConnection connection)
		{
			_connection = connection;
		}
		public async Task<Activity> Add(Activity entity)
		{
			using IDbConnection db = _connection.GetConnection;
			string query = $"insert into public.activity values ({entity.Activity_Id},{entity.Property_Id},'{entity.Activity_Schedule.ToString("yyyy-MM-dd HH:mm:ss")}','{entity.Activity_Title}','{entity.Activity_Created_at.ToString("yyyy-MM-dd HH:mm:ss")}','{entity.Activity_Updated_at.ToString("yyyy-MM-dd HH:mm:ss")}','{entity.Activity_Status}');";


			if (db.State == ConnectionState.Closed)
				db.Open();

			using (var tran = db.BeginTransaction())
			{
				try
				{
					var result = await db.ExecuteScalarAsync<int>(query, null, commandType: CommandType.Text, transaction: tran);
					tran.Commit();
					return new Activity { Activity_Id = result };
				}
				catch (Npgsql.NpgsqlException ex)
				{
					tran.Rollback();
					throw new Exception(ex.Message);
				}
			}
		}

		public Task<Activity> Cancel(Activity entity)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Activity> Exists(int property_id, DateTime schedule)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Activity>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Activity>> GetByFilters(Parameters parameters)
		{
			throw new NotImplementedException();
		}

		public async Task<Activity> GetById(int id)
		{
			string query = @$"SELECT 
                                a.activity_id ,
                                a.property_id,
                                a.activity_schedule,
                                a.activity_title,
                                a.activity_created_at,
                                a.activity_updated_at,
                                a.activity_status,
                                p.property_title,
	                            p.property_address	 
                            FROM activity a     
                            INNER JOIN property p on p.property_id =a.property_id
                            WHERE a.activity_id={id};";
			Activity result = null;
			using (IDbConnection db = _connection.GetConnection)
			{
				if (db.State == ConnectionState.Closed)
					db.Open();

				result = await db.QueryFirstOrDefaultAsync<Activity>(query, null, commandType: CommandType.Text);
				return result;
				//result = await db.QueryAsync<Test.Entities.Activity, Test.Entities.Property, Activity>(
				//   query,
				//   (activity, property) =>
				//   {
				//       activity.Property = property;
				//       return activity;
				//   },
				//   splitOn: "property_Id"
				//   );


				//if (result.AsList<Activity>().Count.Equals(0))
				//{
				//    return null;
				//}
				//var _activity = result.AsList<Activity>()[0];

				//return _activity;

			}

		}

		public Task<Activity> Modify(Activity entity)
		{
			throw new NotImplementedException();
		}

		public Task<Activity> Reschedule(Activity entity)
		{
			throw new NotImplementedException();
		}

		public Task<Activity> Terminate(Activity entity)
		{
			throw new NotImplementedException();
		}
	}
}
