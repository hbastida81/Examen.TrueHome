﻿using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Examen.Data.Connection
{
    public class Connection : IConnection
    {
        private readonly IConfiguration _configuration;
        public Connection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetExConnection
        {
            get
            {
                var sqlConnection = new NpgsqlConnection();
                if (sqlConnection == null)
                    return null;

                sqlConnection.ConnectionString = _configuration.GetConnectionString("ExConnection");
                sqlConnection.Open();
                return sqlConnection;
            }
        }
    }
    public interface IConnection
    {
        IDbConnection GetExConnection { get; }
    }
}