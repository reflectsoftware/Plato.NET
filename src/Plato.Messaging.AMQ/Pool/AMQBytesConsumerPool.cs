// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Messaging.AMQ.Factories;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Interfaces;

namespace Plato.Messaging.AMQ.Pool
{
    public interface IAMQPoolFactory 
    {
        T Create<T>() where T : IMessageReceiverSender;
    }

    public class AMQPoolSenderReceiverFactory
    {
        private IAMQSenderFactory _senderFactory;
        private IAMQReceiverFactory _receiverFactory;
        
        public AMQPoolSenderReceiverFactory(IAMQConnectionFactory connectionFactory)
        {
            _senderFactory = new AMQSenderFactory(connectionFactory);
            _receiverFactory = new AMQReceiverFactory(connectionFactory);
        }

        public T Create<T>(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings) where T : IMessageReceiverSender
        {
            if (typeof(T) == typeof(IAMQReceiverBytes))
            {
                return (T)_receiverFactory.CreateBytes(connectionSettings, destinationSettings);
            }

            return default(T);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="GenericObjectPool{Plato.Messaging.AMQ.Interfaces.IAMQReceiverBytes, Plato.Messaging.AMQ.Pool.AMQPoolStates}" />
    internal class AMQBytesConsumerPool : GenericObjectPool<IAMQReceiverBytes, AMQPoolStates>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQBytesConsumerPool"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQBytesConsumerPool(
            AMQPoolStates states,             
            int initialPoolSize,
            int maxGrowSize) : base(initialPoolSize, maxGrowSize, states)
        {
        }

        /// <summary>
        /// Creates the pool object.
        /// </summary>
        /// <returns></returns>
        protected override IAMQReceiverBytes CreatePoolObject()
        {
            return Data.ReceiverFactory.CreateBytes(Data.Connection, Data.Destination);
        }
    }
}
