// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.AMQ.Interfaces;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.Pool.AMQPoolBase" />
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQPool" />
    public class AMQPool: AMQPoolBase, IAMQPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQPool"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="senderFactory">The sender factory.</param>
        /// <param name="receiverFactory">The receiver factory.</param>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQPool(
            IAMQConfigurationManager configurationManager, 
            IAMQSenderFactory senderFactory, 
            IAMQReceiverFactory receiverFactory,
            int initialPoolSize,
            int maxGrowSize) : base(configurationManager, senderFactory, receiverFactory, initialPoolSize, maxGrowSize)
        {
        }

        /// <summary>
        /// Gets the producer.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public IAMQPoolContainer<IAMQSenderText> GetProducer(string connectionName, string queueName)
        {            
            var states = VerifyPoolStates(connectionName, queueName);            

            var cacheKey = $"amq:pool:producer:{connectionName}:{queueName}".ToLower();
            var pool = _cache.Get(cacheKey, (name, args) =>
            {
                return new CacheDataInfo<AMQProducerPool>
                {
                    NewCacheData = new AMQProducerPool(states, _initialPoolSize, _maxGrowSize)
                };
            });

            var container = pool.Container();
            return new AMQPoolContainer<IAMQSenderText>(container);
        }

        /// <summary>
        /// Gets the consumer.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        public IAMQPoolContainer<IAMQReceiverText> GetConsumer(string connectionName, string queueName)
        {
            var states = VerifyPoolStates(connectionName, queueName);

            var cacheKey = $"amq:pool:consumer:{connectionName}:{queueName}".ToLower();
            var pool = _cache.Get(cacheKey, (name, args) =>
            {
                return new CacheDataInfo<AMQConsumerPool>
                {
                    NewCacheData = new AMQConsumerPool(states, _initialPoolSize, _maxGrowSize)
                };
            });

            var container = pool.Container();
            return new AMQPoolContainer<IAMQReceiverText>(container);
        }
    }
}
