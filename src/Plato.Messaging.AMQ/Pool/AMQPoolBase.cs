// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Cache;
using Plato.Cache.Interfaces;
using Plato.Core.Miscellaneous;
using Plato.Messaging.AMQ.Interfaces;
using Plato.Messaging.AMQ.Settings;
using System;

namespace Plato.Messaging.AMQ.Pool
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class AMQPoolBase : IDisposable
    {
        protected class ConfigStates
        {
            public AMQConnectionSettings Connection { get; set; }
            public AMQDestinationSettings Destination { get; set; }
        }

        private bool _disposed;        
        protected readonly int _maxGrowSize;
        protected readonly ILocalMemoryCache _cache;
        protected readonly IAMQConfigurationManager _configurationManager;
        protected readonly IAMQSenderReceiverFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQPoolBase"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="maxGrowSize">Maximum size of the grow.</param>
        public AMQPoolBase(
            IAMQConfigurationManager configurationManager,
            IAMQSenderReceiverFactory factory,            
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
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        protected ConfigStates VerifyPoolStates(string connectionName, string queueName)
        {
            var states = new ConfigStates
            {
                Connection = _configurationManager.GetConnectionSettings(connectionName),
                Destination = _configurationManager.GetDestinationSettings(queueName),
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
