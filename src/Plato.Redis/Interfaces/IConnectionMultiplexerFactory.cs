// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;
using System.IO;
using System.Threading.Tasks;

namespace Plato.Redis.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConnectionMultiplexerFactory
    {
        /// <summary>
        /// Connects the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        ConnectionMultiplexer Connect(ConfigurationOptions options, string connectionStrings, TextWriter log = null);

        /// <summary>
        /// Connects the specified connection strings.
        /// </summary>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        ConnectionMultiplexer Connect(string connectionStrings, TextWriter log = null);

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        Task<ConnectionMultiplexer> ConnectAsync(ConfigurationOptions options, string connectionStrings, TextWriter log = null);

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="connectionStrings">The connection strings.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        Task<ConnectionMultiplexer> ConnectAsync(string connectionStrings, TextWriter log = null);
    }
}
