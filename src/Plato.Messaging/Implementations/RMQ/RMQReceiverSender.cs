// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public abstract class RMQReceiverSender : IMessageReceiverSender
    {
        protected readonly RMQSettings _settings;
        protected readonly TimeoutException _timeoutException;
        protected IConnection _connection;
        protected IModel _channel;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IMessageReceiverSender" /> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverSender"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RMQReceiverSender(RMQSettings settings)
        {
            Disposed = false;
            _settings = settings;
            _channel = null;
            _timeoutException = new TimeoutException();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RMQReceiverSender"/> class.
        /// </summary>
        ~RMQReceiverSender()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    Close();
                }
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        protected void OpenConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _settings.ConnectionFactory.CreateConnection(_settings.ConnectionName);
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        protected void CloseConnection()
        {
            if (_connection != null)
            {
                _settings.ConnectionFactory.RemoveConnection(_settings.ConnectionName);
                _connection = null;
            }
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        protected void OpenChannel()
        {
            try
            {
                if (_channel == null || !_channel.IsOpen)
                {
                    _channel = _connection.CreateModel();

                    if (_settings.ExchangeSettings != null 
                    && !string.IsNullOrWhiteSpace(_settings.ExchangeSettings.ExchangeName))
                    {
                        _channel.ExchangeDeclare(_settings.ExchangeSettings.ExchangeName, 
                            _settings.ExchangeSettings.Type, 
                            _settings.ExchangeSettings.Durable, 
                            _settings.ExchangeSettings.AutoDelete, 
                            _settings.ExchangeSettings.Arguments);
                    }
                }
            }
            catch (Exception ex)
            {
                var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }

        /// <summary>
        /// Closes the channel.
        /// </summary>
        protected void CloseChannel()
        {
            if (_channel == null)
            {
                return;
            }

            try
            {
                if (_channel.IsOpen)
                {
                    _channel.Close();                    
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _channel?.Dispose();
                _channel = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Determines whether this instance is open.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOpen()
        {
            return _channel != null && _channel.IsOpen;
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public virtual void Open()
        {
            if (IsOpen())
            {
                return;
            }

            OpenConnection();
            OpenChannel();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {
            CloseChannel();
            CloseConnection();
        }
    }
}
