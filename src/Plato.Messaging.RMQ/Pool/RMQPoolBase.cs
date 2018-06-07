// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Cache.Interfaces;
using Plato.Core.Miscellaneous;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using System;
using System.Collections.Generic;

namespace Plato.Messaging.RMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class RMQPoolBase : IDisposable
    {
        protected class ConfigStates
        {
            public RMQConnectionSettings Connection { get; set; }
            public RMQQueueSettings Destination { get; set; }
            public RMQExchangeSettings Exchange { get; set; }
        }

        private bool _disposed;        
        protected readonly int _maxGrowSize;
        protected readonly ILocalMemoryCache _cache;
        protected readonly IRMQConfigurationManager _configurationManager;
        protected readonly IRMQSenderReceiverFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQPoolBase"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public RMQPoolBase(
            IRMQConfigurationManager configurationManager,
            IRMQSenderReceiverFactory factory,            
            int maxGrowSize)
        {
            Guard.AgainstNull(() => configurationManager);
            Guard.AgainstNull(() => factory);            

            _disposed = false;
            _cache = new LocalMemoryCache();            
            _maxGrowSize = maxGrowSize;
            _configurationManager = configurationManager;
            _factory = factory;
        }

        /// <summary>
        /// Verifies the pool states.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="queueArgs">The queue arguments.</param>
        /// <param name="exchangeArgs">The exchange arguments.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        protected ConfigStates VerifyPoolStates(
            string connectionName, 
            string queueName, 
            string exchangeName = null,
            IDictionary<string, object> queueArgs = null,
            IDictionary<string, object> exchangeArgs = null)
        {
            var states = new ConfigStates
            {
                Connection = _configurationManager.GetConnectionSettings(connectionName),
                Destination = _configurationManager.GetQueueSettings(queueName),
                Exchange = string.IsNullOrWhiteSpace(exchangeName) ? null : _configurationManager.GetExchangeSettings(exchangeName),
            };

            if (string.IsNullOrWhiteSpace(states.Connection.Uri))
            {
                throw new Exception($"Connection: {connectionName} is not configured or is missing from configuration.");
            }

            if (string.IsNullOrWhiteSpace(states.Destination.QueueName))
            {
                throw new Exception($"Queue: {queueName} is not configured or is missing from configuration.");
            }

            if(states.Exchange != null && string.IsNullOrWhiteSpace(states.Exchange.ExchangeName))
            {
                throw new Exception($"Exchange: {exchangeName} is not configured or is missing from configuration.");
            }

            return states;
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="RMQPoolBase"/> class.
        /// </summary>
        ~RMQPoolBase()
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
