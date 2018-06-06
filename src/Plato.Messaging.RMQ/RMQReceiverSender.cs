﻿// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace Plato.Messaging.RMQ
{
    public abstract class RMQReceiverSender : IMessageReceiverSender
    {
        protected static TimeoutException _TimeoutException;
        
        protected readonly IRMQConnectionFactory _connectionFactory;
        protected readonly RMQConnectionSettings _connectionSettings;
        protected IConnection _connection;
        protected IModel _channel;
        protected DateTime _lastConnectionDateTime;
        
        public bool Disposed { get; private set; }
        public Guid Id { get; private set; }
    
        /// <summary>
        /// Initializes the <see cref="RMQReceiverSender"/> class.
        /// </summary>
        static RMQReceiverSender()
        {
            _TimeoutException = new TimeoutException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQReceiverSender"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="connectionSettings">The connection settings.</param>
        public RMQReceiverSender(IRMQConnectionFactory connectionFactory, RMQConnectionSettings connectionSettings)
        {
            Id = Guid.NewGuid();
            Disposed = false;   
            
            _lastConnectionDateTime = DateTime.MinValue;
            _connectionFactory = connectionFactory;
            _connectionSettings = connectionSettings;
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
        /// Forces the reconnection.
        /// </summary>
        protected void ForceReconnection()
        {
            if(_connectionSettings.ForceReconnectionTime == TimeSpan.Zero)
            {
                return;
            }

            if (DateTime.Now.Subtract(_lastConnectionDateTime) > _connectionSettings.ForceReconnectionTime)
            {
                Close();
                _lastConnectionDateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        protected virtual void OpenConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection(_connectionSettings);
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        protected virtual void CloseConnection()
        {
            try
            {
                _connection?.Close();
            }
            catch (AlreadyClosedException)
            {
                // just swallow as it's safe to continue with the close process.
            }
            finally
            {
                _connection?.Dispose();
                _connection = null;
            }
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
            ForceReconnection();

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
