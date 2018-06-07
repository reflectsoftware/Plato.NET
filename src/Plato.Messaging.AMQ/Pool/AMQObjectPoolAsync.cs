// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using System;
using System.Threading.Tasks;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Cache.GenericObjectPoolAsync{Plato.Messaging.AMQ.Pool.AMQObjectPoolData, System.Object}" />
    internal class AMQObjectPoolAsync : GenericObjectPoolAsync<AMQObjectPoolData, object>
    {
        private readonly IAMQSenderReceiverFactory _factory;
        private readonly Type _instanceType;
        private readonly AMQConnectionSettings _connection;
        private readonly AMQDestinationSettings _destination;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQObjectPool" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQObjectPoolAsync(
            IAMQSenderReceiverFactory factory,
            Type instanceType,
            AMQConnectionSettings connection,
            AMQDestinationSettings destination,
            int maxGrowSize) : base(0, maxGrowSize)
        {
            _factory = factory;
            _instanceType = instanceType;
            _connection = connection;
            _destination = destination;
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override Task<AMQObjectPoolData> CreatePoolObjectAsync()
        {
            return Task.FromResult(new AMQObjectPoolData { Instance = _factory.Create(_instanceType, _connection, _destination) });
        }
    }
}

