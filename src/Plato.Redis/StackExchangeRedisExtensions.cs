// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using StackExchange.Redis;

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public static class StackExchangeRedisExtensions
    {
        /// <summary>
        /// Gets the database proxy.
        /// </summary>
        /// <param name="muxer">The muxer.</param>
        /// <param name="db">The database.</param>
        /// <param name="asyncState">State of the asynchronous.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IDatabase GetDatabaseProxy(this IConnectionMultiplexer muxer, int db = -1, object asyncState = null, RedisDatabaseProxyConfiguration config = null)
        {
            var database = muxer.GetDatabase(db, asyncState);
            return new RedisDatabaseProxy(database, config).GetTransparentProxy() as IDatabase;
        }        
    }
}
