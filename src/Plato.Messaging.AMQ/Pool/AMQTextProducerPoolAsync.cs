﻿// Plato.NET
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
    /// <seealso cref="Plato.Cache.GenericObjectPoolAsync{Plato.Messaging.AMQ.Interfaces.IAMQSenderText, Plato.Messaging.AMQ.Pool.AMQPoolStates}" />
    internal class AMQTextProducerPoolAsync : GenericObjectPoolAsync<IAMQSenderText, AMQPoolStates>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQTextProducerPoolAsync"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQTextProducerPoolAsync(
            AMQPoolStates states,             
            int initialPoolSize, 
            int maxGrowSize) : base(initialPoolSize, maxGrowSize, states)
        {                   
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override Task<IAMQSenderText> CreatePoolObjectAsync()
        {
            return Task.FromResult(Data.SenderFactory.CreateText(Data.Connection, Data.Destination));
        }        
    }
}
