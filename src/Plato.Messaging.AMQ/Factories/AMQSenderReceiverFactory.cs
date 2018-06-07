// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.AMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQSenderReceiverFactory" />
    public class AMQSenderReceiverFactory : IAMQSenderReceiverFactory
    {
        private readonly IAMQSenderFactory _senderFactory;
        private readonly IAMQReceiverFactory _receiverFactory;
        private readonly Dictionary<Type, Func<AMQConnectionSettings, AMQDestinationSettings, IMessageReceiverSender>> _invokers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQSenderReceiverFactory"/> class.
        /// </summary>
        /// <param name="senderFactory">The sender factory.</param>
        /// <param name="receiverFactory">The receiver factory.</param>
        public AMQSenderReceiverFactory(            
            IAMQSenderFactory senderFactory,
            IAMQReceiverFactory receiverFactory)
        {
            _senderFactory = senderFactory;
            _receiverFactory = receiverFactory;

            _invokers = new Dictionary<Type, Func<AMQConnectionSettings, AMQDestinationSettings, IMessageReceiverSender>>();
            PrepareInvokers();
        }

        /// <summary>
        /// Prepares the invokers.
        /// </summary>
        private void PrepareInvokers()
        {
            _invokers[typeof(IAMQSender)] = (connection, destination) => _senderFactory.Create(connection, destination);
            _invokers[typeof(IAMQSenderBytes)] = (connection, destination) => _senderFactory.CreateBytes(connection, destination);
            _invokers[typeof(IAMQSenderText)] = (connection, destination) => _senderFactory.CreateText(connection, destination);

            _invokers[typeof(IAMQReceiver)] = (connection, destination) => _receiverFactory.Create(connection, destination);
            _invokers[typeof(IAMQReceiverBytes)] = (connection, destination) => _receiverFactory.CreateBytes(connection, destination);
            _invokers[typeof(IAMQReceiverText)] = (connection, destination) => _receiverFactory.CreateText(connection, destination);
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="connection">The connection settings.</param>
        /// <param name="destination">The destination settings.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IMessageReceiverSender Create(Type type, AMQConnectionSettings connection, AMQDestinationSettings destination)
        {            
            if (!_invokers.ContainsKey(type))
            {
                throw new KeyNotFoundException($"The following type: '{type.Name}' is not supported.");
            }

            var invoker = _invokers[type];

            return invoker(connection, destination);
        }

        /// <summary>
        /// Creates the specified connection settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection settings.</param>
        /// <param name="destination">The destination settings.</param>
        /// <returns></returns>
        public T Create<T>(AMQConnectionSettings connection, AMQDestinationSettings destination) where T : IMessageReceiverSender
        {
            return (T)Create(typeof(T), connection, destination);
        }
    }
}
