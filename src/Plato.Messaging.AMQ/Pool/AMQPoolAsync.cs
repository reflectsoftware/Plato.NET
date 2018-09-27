// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.Interfaces;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Pool
{
    public class AMQPoolAsync : AMQPoolBase, IAMQPoolAsync
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQPoolAsync"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQPoolAsync(
            IAMQConfigurationManager configurationManager,
            IAMQSenderReceiverFactory factory,
            int maxGrowSize = 3) : base(configurationManager, factory, maxGrowSize)
        {
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public async Task<IAMQPoolContainer<T>> GetAsync<T>(string connectionName, string queueName) where T : IMessageReceiverSender
        {
            var states = VerifyPoolStates(connectionName, queueName);
            var type = typeof(T);

            var cacheKey = $"type:{type.Name}:{connectionName}:{queueName}".ToLower();
            var pool = await _cache.GetAsync(cacheKey, (name, args) =>
            {
                var objectPool = new AMQObjectPoolAsync(_factory, type, states.Connection, states.Destination, _maxGrowSize);
                return Task.FromResult(new CacheDataInfo<AMQObjectPoolAsync> { NewCacheData = objectPool });
            });

            var container = await pool.ContainerAsync();
            return new AMQPoolContainerAsync<T>(container);
        }
    }
}
