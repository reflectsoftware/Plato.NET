// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Data;
using System.Threading.Tasks;

namespace Plato.SqlServer
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        private readonly DbExecuteWrapper _dbExecuteWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            _dbExecuteWrapper = new DbExecuteWrapper();
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        protected async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> operation)
        {
            return await _dbExecuteWrapper.ExecuteAsync<T>(_connectionString, operation);
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        protected async Task ExecuteAsync(Func<IDbConnection, Task> operation)
        {
            await _dbExecuteWrapper.ExecuteAsync(_connectionString, operation);
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        protected T Execute<T>(Func<IDbConnection, T> operation)
        {
            return _dbExecuteWrapper.Execute<T>(_connectionString, operation);
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        protected void Execute(Action<IDbConnection> operation)
        {
            _dbExecuteWrapper.Execute(_connectionString, operation);
        }
    }
}
