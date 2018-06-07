// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.Interfaces;
using System.Collections.Generic;

namespace Plato.Messaging.RMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.Pool.RMQPoolBase" />
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQPool" />
    public class RMQPool: RMQPoolBase, IRMQPool
    {
        public RMQPool(
            IRMQConfigurationManager configurationManager,
            IRMQSenderReceiverFactory factory,
            int maxGrowSize) : base(configurationManager, factory, maxGrowSize)
        {
        }

        /// <summary>
        /// Gets the specified connection name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="queueArgs">The queue arguments.</param>
        /// <param name="exchangeArgs">The exchange arguments.</param>
        /// <returns></returns>
        public IRMQPoolContainer<T> Get<T>(
            string connectionName, 
            string queueName, 
            string exchangeName = null,
            IDictionary<string, object> queueArgs = null,
            IDictionary<string, object> exchangeArgs = null) where T: IMessageReceiverSender
        {
            var states = VerifyPoolStates(connectionName, queueName, exchangeName);
            var type = typeof(T);

            var cacheKey = $"type:{type.Name}:{connectionName}:{queueName}.{exchangeName ?? "(null)"}".ToLower();
            var pool = _cache.Get(cacheKey, (name, args) =>
            {
                if(queueArgs != null)
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

                var objectPool = new RMQObjectPool(_factory, type, states.Connection, states.Destination, states.Exchange, _maxGrowSize);
                return new CacheDataInfo<RMQObjectPool> { NewCacheData = objectPool };
            });

            var container = pool.Container();
            return new RMQPoolContainer<T>(container);
        }
    }
}
