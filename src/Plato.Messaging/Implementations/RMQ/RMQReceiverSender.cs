// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
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
        protected static TimeoutException _TimeoutException;

        protected readonly IRMQConnectionFactory _connectionFactory;
        protected readonly string _connectionName;
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
        /// Initializes the <see cref="RMQReceiverSender" /> class.
        /// </summary>
        static RMQReceiverSender()
        {
            _TimeoutException = new TimeoutException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverSender"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionName">Name of the connection.</param>
        public RMQReceiverSender(
            IRMQConnectionFactory connectionFactory, 
            string connectionName)
        {
            Disposed = false;
            _connectionFactory = connectionFactory;
            _connectionName = connectionName;
            _channel = null;            
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        protected virtual void OpenConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection(_connectionName);
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        protected virtual void CloseConnection()
        {
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }

        /// <summary>
        /// Opens the channel.
        /// </summary>
        protected virtual void OpenChannel()
        {
            try
            {
                if (_channel == null || !_channel.IsOpen)
                {
                    _channel = _connection.CreateModel();
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
        protected virtual void CloseChannel()
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
