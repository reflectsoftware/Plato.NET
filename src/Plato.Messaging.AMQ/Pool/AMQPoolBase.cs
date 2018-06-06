// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Cache.Interfaces;
using Plato.Core.Miscellaneous;
using Plato.Messaging.AMQ.Interfaces;
using System;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class AMQPoolBase : IDisposable
    {
        private bool _disposed;
        protected readonly int _initialPoolSize;
        protected readonly int _maxGrowSize;
        protected readonly ILocalMemoryCache _cache;
        protected readonly IAMQConfigurationManager _configurationManager;
        protected readonly IAMQSenderFactory _senderFactory;
        protected readonly IAMQReceiverFactory _receiverFactory;

        public AMQPoolBase(
            IAMQConfigurationManager configurationManager,
            IAMQSenderFactory senderFactory,
            IAMQReceiverFactory receiverFactory,
            int initialPoolSize,
            int maxGrowSize)
        {
            Guard.AgainstNull(() => configurationManager);
            Guard.AgainstNull(() => senderFactory);
            Guard.AgainstNull(() => receiverFactory);

            _disposed = false;
            _cache = new LocalMemoryCache();
            _initialPoolSize = initialPoolSize;
            _maxGrowSize = maxGrowSize;
            _configurationManager = configurationManager;
            _senderFactory = senderFactory;
            _receiverFactory = receiverFactory;
        }

        /// <summary>
        /// Verifies the pool states.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        protected AMQPoolStates VerifyPoolStates(string connectionName, string queueName)
        {
            var states = new AMQPoolStates
            {
                Connection = _configurationManager.GetConnectionSettings(connectionName),
                Destination = _configurationManager.GetDestinationSettings(queueName),
                SenderFactory = _senderFactory,
                ReceiverFactory = _receiverFactory
            };

            if (string.IsNullOrWhiteSpace(states.Connection.Uri))
            {
                throw new Exception($"Connection: {connectionName} is not configured or is missing from configuration.");
            }

            if (string.IsNullOrWhiteSpace(states.Destination.Path))
            {
                throw new Exception($"Queue: {queueName} is not configured or is missing from configuration.");
            }

            return states;
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="AMQPoolBase"/> class.
        /// </summary>
        ~AMQPoolBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    GC.SuppressFinalize(this);

                    _cache?.Dispose();
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion Dispose
    }
}
