// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Plato.SQL
{
    /// <summary>
    ///
    /// </summary>
    public class DbExecuteWrapper
    {
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_connectionString">The _connection string.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        public async Task<T> ExecuteAsync<T>(string _connectionString, Func<IDbConnection, Task<T>> operation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await operation(connection);
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="_connectionString">The _connection string.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// ExecuteAsync() experienced a SQL timeout.
        /// or
        /// ExecuteAsync() experienced a SQL exception (not a timeout).
        /// </exception>
        public async Task ExecuteAsync(string _connectionString, Func<IDbConnection, Task> operation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await operation(connection);
            }
        }

        /// <summary>
        /// Executes the specified _connection string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_connectionString">The _connection string.</param>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        public T Execute<T>(string _connectionString, Func<IDbConnection, T> operation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return operation(connection);
            }
        }

        /// <summary>
        /// Executes the specified _connection string.
        /// </summary>
        /// <param name="_connectionString">The _connection string.</param>
        /// <param name="operation">The operation.</param>
        /// <exception cref="System.Exception">
        /// Execute() experienced a SQL timeout.
        /// or
        /// Execute() experienced a SQL exception (not a timeout).
        /// </exception>
        public void Execute(string _connectionString, Action<IDbConnection> operation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                operation(connection);
            }
        }
    }
}
