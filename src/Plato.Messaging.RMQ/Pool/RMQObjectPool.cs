// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using System;

namespace Plato.Messaging.RMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Cache.GenericObjectPool{Plato.Messaging.RMQ.Pool.RMQObjectPoolData, System.Object}" />
    internal class RMQObjectPool : GenericObjectPool<RMQObjectPoolData, object>
    {
        private readonly IRMQSenderReceiverFactory _factory;
        private readonly Type _instanceType;
        private readonly RMQConnectionSettings _connection;
        private readonly RMQQueueSettings _destination;
        private readonly RMQExchangeSettings _exchange;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQObjectPoolData" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public RMQObjectPool(
            IRMQSenderReceiverFactory factory,
            Type instanceType,
            RMQConnectionSettings connection,
            RMQQueueSettings destination,
            RMQExchangeSettings exchange,
            int maxGrowSize) : base(0, maxGrowSize)
        {
            _factory = factory;
            _instanceType = instanceType;
            _connection = connection;
            _destination = destination;
            _exchange = exchange;
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override RMQObjectPoolData CreatePoolObject()
        {
            var instance = _exchange == null
                ? _factory.Create(_instanceType, _connection, _destination)
                : _factory.Create(_instanceType, _connection, _exchange, _destination);

            return new RMQObjectPoolData { Instance = instance };
        }
    }
}

