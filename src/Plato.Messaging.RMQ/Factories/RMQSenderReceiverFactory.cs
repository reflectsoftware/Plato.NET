// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQSenderReceiverFactory" />
    public class RMQSenderReceiverFactory : IRMQSenderReceiverFactory
    {
        private readonly IRMQConsumerFactory _consumerFactory;
        private readonly IRMQProducerFactory _producerFactory;
        private readonly IRMQPublisherFactory _publisherFactory;
        private readonly IRMQSubscriberFactory _subscriberFactory;
        private readonly Dictionary<Type, Func<RMQConnectionSettings, RMQQueueSettings, IMessageReceiverSender>> _invokers;
        private readonly Dictionary<Type, Func<RMQConnectionSettings, RMQExchangeSettings, IMessageReceiverSender>> _pubInvokers;
        private readonly Dictionary<Type, Func<RMQConnectionSettings, RMQExchangeSettings, RMQQueueSettings, IMessageReceiverSender>> _subInvokers;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQSenderReceiverFactory" /> class.
        /// </summary>
        /// <param name="consumerFactory">The consumer factory.</param>
        /// <param name="producerFactory">The producer factory.</param>
        /// <param name="subscriberFactor">The subscriber factor.</param>
        /// <param name="publisherFactory">The publisher factory.</param>
        public RMQSenderReceiverFactory(
            IRMQConsumerFactory consumerFactory,
            IRMQProducerFactory producerFactory,
            IRMQSubscriberFactory subscriberFactory,
            IRMQPublisherFactory publisherFactory)
        {
            _consumerFactory = consumerFactory;
            _producerFactory = producerFactory;
            _subscriberFactory = subscriberFactory;
            _publisherFactory = publisherFactory;
            
            _invokers = new Dictionary<Type, Func<RMQConnectionSettings, RMQQueueSettings, IMessageReceiverSender>>();
            _pubInvokers = new Dictionary<Type, Func<RMQConnectionSettings, RMQExchangeSettings, IMessageReceiverSender>>();
            _subInvokers = new Dictionary<Type, Func<RMQConnectionSettings, RMQExchangeSettings, RMQQueueSettings, IMessageReceiverSender>>();

            PrepareInvokers();
        }

        /// <summary>
        /// Prepares the invokers.
        /// </summary>
        private void PrepareInvokers()
        {
            _invokers[typeof(IRMQConsumerBytes)] = (connection, destination) => _consumerFactory.CreateBytes(connection, destination);
            _invokers[typeof(IRMQConsumerText)] = (connection, destination) => _consumerFactory.CreateText(connection, destination);

            _invokers[typeof(IRMQProducerBytes)] = (connection, destination) => _producerFactory.CreateBytes(connection, destination);
            _invokers[typeof(IRMQProducerText)] = (connection, destination) => _producerFactory.CreateText(connection, destination);

            _pubInvokers[typeof(IRMQPublisherBytes)] = (connection, exchange) => _publisherFactory.CreateBytes(connection, exchange);
            _pubInvokers[typeof(IRMQPublisherText)] = (connection, exchange) => _publisherFactory.CreateText(connection, exchange);

            _subInvokers[typeof(IRMQSubscriberBytes)] = (connection, exchange, destination) => _subscriberFactory.CreateBytes(connection, exchange, destination);
            _subInvokers[typeof(IRMQSubscriberText)] = (connection, exchange, destination) => _subscriberFactory.CreateText(connection, exchange, destination);
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="connection">The connection settings.</param>
        /// <param name="destination">The destination settings.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IMessageReceiverSender Create(Type type, RMQConnectionSettings connection, RMQQueueSettings destination)
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
        public T Create<T>(RMQConnectionSettings connection, RMQQueueSettings destination) where T : IMessageReceiverSender
        {
            return (T)Create(typeof(T), connection, destination);
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="exchange">The exchange.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">The following type: '{type.Name}</exception>
        public IMessageReceiverSender Create(Type type, RMQConnectionSettings connection, RMQExchangeSettings exchange)
        {
            if (!_subInvokers.ContainsKey(type))
            {
                throw new KeyNotFoundException($"The following type: '{type.Name}' is not supported.");
            }

            var invoker = _pubInvokers[type];

            return invoker(connection, exchange);
        }

        /// <summary>
        /// Creates the specified connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="exchange">The exchange.</param>
        /// <returns></returns>
        public T Create<T>(RMQConnectionSettings connection, RMQExchangeSettings exchange) where T : IMessageReceiverSender
        {
            return (T)Create(typeof(T), connection, exchange);
        }

        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IMessageReceiverSender Create(Type type, RMQConnectionSettings connection, RMQExchangeSettings exchange, RMQQueueSettings destination)
        {
            if (!_subInvokers.ContainsKey(type))
            {
                throw new KeyNotFoundException($"The following type: '{type.Name}' is not supported.");
            }

            var invoker = _subInvokers[type];

            return invoker(connection, exchange, destination);
        }

        /// <summary>
        /// Creates the specified connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public T Create<T>(RMQConnectionSettings connection, RMQExchangeSettings exchange, RMQQueueSettings destination) where T : IMessageReceiverSender
        {
            return (T)Create(typeof(T), connection, exchange, destination);
        }
    }
}
