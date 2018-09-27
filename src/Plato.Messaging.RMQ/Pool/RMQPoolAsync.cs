// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.Interfaces;
using Plato.Messaging.RMQ.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plato.Messaging.RMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.Pool.RMQPoolBase" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQPoolAsync" />
    public class RMQPoolAsync : RMQPoolBase, IRMQPoolAsync
    {
        public RMQPoolAsync(
            IRMQConfigurationManager configurationManager,
            IRMQSenderReceiverFactory factory,
            int maxGrowSize = 3) : base(configurationManager, factory, maxGrowSize)
        {
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="queueArgs">The queue arguments.</param>
        /// <param name="exchangeArgs">The exchange arguments.</param>
        /// <returns></returns>
        public async Task<IRMQPoolContainer<T>> GetAsync<T>(
            string connectionName, 
            string queueName, 
            string exchangeName = null,
            IDictionary<string, object> queueArgs = null,
            IDictionary<string, object> exchangeArgs = null) where T : IMessageReceiverSender
        {
            var states = VerifyPoolStates(connectionName, queueName, exchangeName);
            var type = typeof(T);

            var cacheKey = $"type:{type.Name}:{connectionName}:{queueName}.{exchangeName ?? "(null)"}".ToLower();
            var pool = await _cache.GetAsync(cacheKey, (name, args) =>
            {
                if (queueArgs != null)
                {
                    foreach (var key in queueArgs.Keys)
                    {
                        states.Destination.Arguments[key] = queueArgs[key];
                    }
                }

                if (exchangeArgs != null && states.Exchange != null)
                {
                    foreach (var key in exchangeArgs.Keys)
                    {
                        states.Exchange.Arguments[key] = exchangeArgs[key];
                    }
                }

                var objectPool = new RMQObjectPoolAsync(_factory, type, states.Connection, states.Destination, states.Exchange, _maxGrowSize);
                return Task.FromResult(new CacheDataInfo<RMQObjectPoolAsync> { NewCacheData = objectPool });
            });

            var container = await pool.ContainerAsync();
            return new RMQPoolContainerAsync<T>(container);
        }
    }
}
