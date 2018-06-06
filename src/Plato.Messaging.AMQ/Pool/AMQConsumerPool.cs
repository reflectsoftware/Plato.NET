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
    /// <seealso cref="GenericObjectPool{Plato.Messaging.AMQ.Interfaces.IAMQReceiverText, Plato.Messaging.AMQ.Pool.AMQPoolStates}" />
    internal class AMQConsumerPool: GenericObjectPool<IAMQReceiverText, AMQPoolStates>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQConsumerPool"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQConsumerPool(
            AMQPoolStates states,             
            int initialPoolSize,
            int maxGrowSize) : base(initialPoolSize, maxGrowSize, states)
        {
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override IAMQReceiverText CreatePoolObject()
        {
            return Data.ReceiverFactory.CreateText(Data.Connection, Data.Destination);
        }
    }
}
