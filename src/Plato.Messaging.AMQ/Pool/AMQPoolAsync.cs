// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.AMQ.Interfaces;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.Pool.AMQPoolBase" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQPoolAsync" />
    public class AMQPoolAsync : AMQPoolBase, IAMQPoolAsync
    {
        public AMQPoolAsync(
            IAMQConfigurationManager configurationManager,
            IAMQSenderFactory senderFactory,
            IAMQReceiverFactory receiverFactory,
            int initialPoolSize,
            int maxGrowSize) : base(configurationManager, senderFactory, receiverFactory, initialPoolSize, maxGrowSize)
        {
        }

        /// <summary>
        /// Gets the producer asynchronous.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public async Task<IAMQPoolContainerAsync<IAMQSenderText>> GetProducerAsync(string connectionName, string queueName)
        {            
            var states = VerifyPoolStates(connectionName, queueName);            

            var cacheKey = $"amq:pool:producer:{connectionName}:{queueName}".ToLower();
            var pool = await _cache.GetAsync(cacheKey, (name, args) =>
            {
                var item = new CacheDataInfo<AMQProducerPoolAsync>
                {
                    NewCacheData = new AMQProducerPoolAsync(states, _initialPoolSize, _maxGrowSize)
                };

                return Task.FromResult(item);
            });

            var container = await pool.ContainerAsync();
            return new AMQPoolContainerAsync<IAMQSenderText>(container);
        }

        /// <summary>
        /// Gets the consumer asynchronous.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public async Task<IAMQPoolContainerAsync<IAMQReceiverText>> GetConsumerAsync(string connectionName, string queueName)
        {
            var states = VerifyPoolStates(connectionName, queueName);

            var cacheKey = $"amq:pool:consumer:{connectionName}:{queueName}".ToLower();
            var pool = await _cache.GetAsync(cacheKey, (name, args) =>
            {
                var item = new CacheDataInfo<AMQConsumerPoolAsync>
                {
                    NewCacheData = new AMQConsumerPoolAsync(states, _initialPoolSize, _maxGrowSize)
                };

                return Task.FromResult(item);
            });

            var container = await pool.ContainerAsync();
            return new AMQPoolContainerAsync<IAMQReceiverText>(container);
        }
    }
}
