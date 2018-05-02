// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.IO;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Redis.Interfaces.IRedisConnection" />
    public class RedisConnection : IRedisConnection
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="RedisConnection"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IConnectionMultiplexer Connection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnection" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public RedisConnection(ConfigurationOptions options, TextWriter logger = null)
        {
            Disposed = false;
            Connection = ConnectionMultiplexer.Connect(options, logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnection" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public RedisConnection(string connectionString, ConfigurationOptions options, TextWriter logger = null)
        {
            Disposed = false;
            var configOptions = options ?? new ConfigurationOptions { AbortOnConnectFail = false };

            foreach (var url in connectionString.Split(';'))
            {
                configOptions.EndPoints.Add(url);
            }

            Connection = ConnectionMultiplexer.Connect(configOptions, logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="logger">The logger.</param>
        public RedisConnection(string connectionString, TextWriter logger = null)
        {
            Disposed = false;
            Connection = ConnectionMultiplexer.Connect(connectionString, logger);
        }

        #region IDisposable
        /// <summary>
        /// Finalizes an instance of the <see cref="RedisConnection"/> class.
        /// </summary>
        ~RedisConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    GC.SuppressFinalize(this);
                    Connection?.Dispose();                    
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {            
            Dispose(true);
        }
        #endregion

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="asyncState">State of the asynchronous.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public IDatabase GetDatabase(int db = -1, object asyncState = null, RedisDatabaseProxyConfiguration config = null)
        {
            return Connection.GetDatabaseProxy(db, asyncState, config);
        }
    }
}
