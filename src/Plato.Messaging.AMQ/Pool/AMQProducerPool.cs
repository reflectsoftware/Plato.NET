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
    /// <seealso cref="Plato.Cache.GenericObjectPool{Plato.Messaging.AMQ.Interfaces.IAMQSenderText, Plato.Messaging.AMQ.Pool.AMQPoolStates}" />
    internal class AMQProducerPool: GenericObjectPool<IAMQSenderText, AMQPoolStates>
    {
        public AMQProducerPool(
            AMQPoolStates states,             
            int initialPoolSize, 
            int maxGrowSize) : base(initialPoolSize, maxGrowSize, states)
        {                   
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override IAMQSenderText CreatePoolObject()
        {
            return Data.SenderFactory.CreateText(Data.Connection, Data.Destination);
        }        
    }
}
