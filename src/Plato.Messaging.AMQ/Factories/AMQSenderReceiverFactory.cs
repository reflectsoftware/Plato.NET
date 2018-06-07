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
        private IAMQSenderFactory _senderFactory;
        private IAMQReceiverFactory _receiverFactory;
        private Dictionary<Type, Func<AMQConnectionSettings, AMQDestinationSettings, IMessageReceiverSender>> _invokers;

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
            _invokers[typeof(IAMQSender)] = (connectionSettings, destinationSettings) => _senderFactory.Create(connectionSettings, destinationSettings);
            _invokers[typeof(IAMQSenderBytes)] = (connectionSettings, destinationSettings) => _senderFactory.CreateBytes(connectionSettings, destinationSettings);
            _invokers[typeof(IAMQSenderText)] = (connectionSettings, destinationSettings) => _senderFactory.CreateText(connectionSettings, destinationSettings);

            _invokers[typeof(IAMQReceiver)] = (connectionSettings, destinationSettings) => _receiverFactory.Create(connectionSettings, destinationSettings);
            _invokers[typeof(IAMQReceiverBytes)] = (connectionSettings, destinationSettings) => _receiverFactory.CreateBytes(connectionSettings, destinationSettings);
            _invokers[typeof(IAMQReceiverText)] = (connectionSettings, destinationSettings) => _receiverFactory.CreateText(connectionSettings, destinationSettings);
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IMessageReceiverSender Create(Type type, AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings)
        {            
            if (!_invokers.ContainsKey(type))
            {
                throw new KeyNotFoundException($"The following type: '{type.Name}' is not supported.");
            }

            var invoker = _invokers[type];

            return invoker(connectionSettings, destinationSettings);
        }

        /// <summary>
        /// Creates the specified connection settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="destinationSettings">The destination settings.</param>
        /// <returns></returns>
        public T Create<T>(AMQConnectionSettings connectionSettings, AMQDestinationSettings destinationSettings) where T : IMessageReceiverSender
        {
            return (T)Create(typeof(T), connectionSettings, destinationSettings);
        }
    }
}
