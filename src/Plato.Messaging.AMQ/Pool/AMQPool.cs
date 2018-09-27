// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.Interfaces;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.Pool.AMQPoolBase" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQPool" />
    public class AMQPool: AMQPoolBase, IAMQPool
    {
        public AMQPool(
            IAMQConfigurationManager configurationManager,
            IAMQSenderReceiverFactory factory,
            int maxGrowSize = 3) : base(configurationManager, factory, maxGrowSize)
        {
        }

        /// <summary>
        /// Gets the specified connection name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public IAMQPoolContainer<T> Get<T>(string connectionName, string queueName) where T: IMessageReceiverSender
        {
            var states = VerifyPoolStates(connectionName, queueName);
            var type = typeof(T);

            var cacheKey = $"type:{type.Name}:{connectionName}:{queueName}".ToLower();
            var pool = _cache.Get(cacheKey, (name, args) =>
            {
                var objectPool = new AMQObjectPool(_factory, type, states.Connection, states.Destination, _maxGrowSize);
                return new CacheDataInfo<AMQObjectPool> { NewCacheData = objectPool };
            });

            var container = pool.Container();
            return new AMQPoolContainer<T>(container);
        }
    }
}
