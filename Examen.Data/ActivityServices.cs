﻿using Examen.Data.Connection;
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

		public async Task<Activity> Cancel(Activity entity)
		{
			using (IDbConnection db = _connection.GetConnection)
			{
				string query = @$"UPDATE activity set activity_status ='Cancelled', activity_updated_at =now() 
                                  WHERE activity_id ={entity.Activity_Id}";

				if (db.State == ConnectionState.Closed)
					db.Open();

				using (var tran = db.BeginTransaction())
				{
					try
					{
						var result = await db.ExecuteAsync(query, null, commandType: CommandType.Text, transaction: tran);
						tran.Commit();
						return new Activity { Activity_Id = entity.Activity_Id };
					}
					catch (Npgsql.NpgsqlException ex)
					{
						tran.Rollback();
						throw new Exception(ex.Message);
					}
				}
			}
		}

		public async Task<int> Delete(int id)
		{
			using (IDbConnection db = _connection.GetConnection)
			{
				string query = @$"Delete public.activity WHERE activity_id = {id}";

				if (db.State == ConnectionState.Closed)
					db.Open();

				using (var tran = db.BeginTransaction())
				{
					try
					{
						var result = await db.ExecuteAsync(query, null, commandType: CommandType.Text, transaction: tran);
						tran.Commit();
						return result;
					}
					catch (Npgsql.NpgsqlException ex)
					{
						tran.Rollback();
						throw new Exception(ex.Message);
					}
				}
			}
		}

		public async Task<Activity> Exists(int property_id, DateTime schedule)
		{
			string query = @$"select 
                                a.activity_id ,
                                a.property_id,
                                a.activity_schedule,
                                a.activity_title,
                                a.activity_created_at,
                                a.activity_updated_at,
                                a.activity_status
                            from activity a 
                            where a.property_id ={property_id}
                            and (to_timestamp('{schedule.ToString("yyyy-MM-dd HH:mm")}','YYYY-MM-DD HH24:MI')>= to_timestamp(to_char(a.activity_schedule,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')
                            and to_timestamp('{schedule.ToString("yyyy-MM-dd HH:mm")}','YYYY-MM-DD HH24:MI')<= to_timestamp(to_char(a.activity_schedule,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')  + interval '1 hours');";

			Activity result = null;
			using (IDbConnection db = _connection.GetConnection)
			{
				if (db.State == ConnectionState.Closed)
					db.Open();

				result = await db.QueryFirstOrDefaultAsync<Activity>(query);

				return result;
			}
		}

		public async Task<IEnumerable<Activity>> GetAll()
		{
			string query = @"SELECT a.activity_id,	                            
								a.activity_schedule,
								a.activity_title,
								a.activity_created_at,
								a.activity_updated_at,
								a.activity_status,
								p.property_id ,	                            
								p.property_title,
								p.property_address,
								p.property_description,
								p.property_updated_at,
								p.property_disabled_at,
								p.property_status   
			                                   FROM  activity a
                        INNER JOIN property p on p.property_id =a.property_id";

			IEnumerable<Activity> result = null;
			using (IDbConnection db = _connection.GetConnection)
			{
				if (db.State == ConnectionState.Closed)
					db.Open();

				result = await db.QueryAsync<Activity>(query, null, commandType: CommandType.Text);
				return result;
			}
		}

		public async Task<IEnumerable<Activity>> GetByFilters(Parameters parameters)
		{
			string p1 = string.Empty;
			string p2 = string.Empty;
			if (parameters.StartDate.HasValue && parameters.EndDate.HasValue) // verificamos que traiga el dato
			{
				p1 += @$"AND (to_timestamp(to_char(a.activity_schedule,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI') >= to_timestamp('{parameters.StartDate.Value.ToString("yyyy-MM-dd HH:mm")}','YYYY-MM-DD HH24:MI')
                        AND to_timestamp(to_char(a.activity_schedule,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI') <= to_timestamp('{parameters.EndDate.Value.ToString("yyyy-MM-dd HH:mm")}', 'YYYY-MM-DD HH24:MI') )";
			}

			if (!string.IsNullOrEmpty(parameters.Status))
			{
				p2 = $"AND a.activity_status ='{parameters.Status}' ";
			}

			var query = @$"SELECT 
	                        a.activity_id ,	
	                        a.activity_schedule,
	                        a.activity_title,
	                        a.activity_created_at,	
	                        case 
		                        when a.activity_status='Enabled' and a.activity_schedule>= now()  then
			                        'Pendiente'
		                        when a.activity_status='Enabled' and a.activity_schedule< now()  then
			                        'Atrasada'
		                        when a.activity_status='Done'  then
			                        'Finalizada'
		                        else 
		                        a.activity_status 
	                        end activity_status,	
	                        p.property_id ,
	                        p.property_title,
	                        p.property_address	 
                        FROM  activity a
                        INNER JOIN property p on p.property_id =a.property_id
                        WHERE 1=1 {p1} {p2} ";

			IEnumerable<Activity> result = null;
			using (IDbConnection db = _connection.GetConnection)
			{
				if (db.State == ConnectionState.Closed)
					db.Open();

				result = await db.QueryAsync<Activity>(query, null, commandType: CommandType.Text);

				return result;
			}

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


			}

		}

		public Task<Activity> Modify(Activity entity)
		{
			throw new NotImplementedException();
		}

		public async Task<Activity> Reschedule(Activity entity)
		{
			using (IDbConnection db = _connection.GetConnection)
			{
				string query = @$"UPDATE activity set activity_schedule ='{entity.Activity_Schedule.ToString("yyyy-MM-dd HH:mm")}', activity_updated_at =now() 
                                  WHERE activity_id ={entity.Activity_Id}";

				if (db.State == ConnectionState.Closed)
					db.Open();

				using (var tran = db.BeginTransaction())
				{
					try
					{
						var result = await db.ExecuteAsync(query, null, commandType: CommandType.Text, transaction: tran);
						tran.Commit();
						return new Activity { Activity_Id = entity.Activity_Id };
					}
					catch (Npgsql.NpgsqlException ex)
					{
						tran.Rollback();
						throw new Exception(ex.Message);
					}
				}
			}
		}

		public async Task<Activity> Terminate(Activity entity)
		{
			using (IDbConnection db = _connection.GetConnection)
			{
				string query = @$"UPDATE activity set activity_status ='Done', activity_updated_at =now() 
                                  WHERE activity_id ={entity.Activity_Id}";

				if (db.State == ConnectionState.Closed)
					db.Open();

				using (var tran = db.BeginTransaction())
				{
					try
					{
						var result = await db.ExecuteAsync(query, null, commandType: CommandType.Text, transaction: tran);
						tran.Commit();
						return new Activity { Activity_Id = entity.Activity_Id };
					}
					catch (Npgsql.NpgsqlException ex)
					{
						tran.Rollback();
						throw new Exception(ex.Message);
					}
				}
			}
		}
	}
}
