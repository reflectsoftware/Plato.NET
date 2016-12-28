// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Redis.Interfaces;
using StackExchange.Redis;
using System.IO;
using System.Threading.Tasks;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Client1.IConnectionMultiplexerFactory" />
    public class ConnectionMultiplexerFactory : IConnectionMultiplexerFactory
    {
        protected virtual ConfigurationOptions EnhanceConfiguration(ConfigurationOptions options, string connectionStrings)
        {
            options.AbortOnConnectFail = false;

            foreach (var url in connectionStrings.Split(','))
            {
                options.EndPoints.Add(url);
            }

            return options;
        }

        /// <summary>
        /// Connects the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public ConnectionMultiplexer Connect(ConfigurationOptions options, string connectionStrings, TextWriter log = null)
        {
            return ConnectionMultiplexer.Connect(EnhanceConfiguration(options, connectionStrings), log);
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public async Task<ConnectionMultiplexer> ConnectAsync(ConfigurationOptions options, string connectionStrings, TextWriter log = null)
        {
            return await ConnectionMultiplexer.ConnectAsync(EnhanceConfiguration(options, connectionStrings), log);
        }

        /// <summary>
        /// Connects the specified connection strings.
        /// </summary>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public ConnectionMultiplexer Connect(string connectionStrings, TextWriter log = null)
        {
            return Connect(new ConfigurationOptions(), connectionStrings, log);
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        public Task<ConnectionMultiplexer> ConnectAsync(string connectionStrings, TextWriter log = null)
        {
            return ConnectAsync(new ConfigurationOptions(), connectionStrings, log);
        }
    }
}
