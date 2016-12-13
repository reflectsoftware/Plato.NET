// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQQueue : IMessageReceiverSender
    {
        protected static TimeoutException _TimeoutException;

        protected readonly RMQQueueSettings _settings;
        protected readonly IRMQConnectionFactory _connctionFactory;
        protected readonly string _connectionName;
        protected IConnection _connection;
        protected IModel _channel;

        public bool Disposed { get; private set; }

        static RMQQueue()
        {
            _TimeoutException = new TimeoutException();
        }
        
        public RMQQueue(IRMQConnectionFactory connctionFactory, string connectionName, RMQQueueSettings settings)
        {
            Disposed = false;
            _connctionFactory = connctionFactory;
            _connectionName = connectionName;
            _settings = settings;
            _channel = null;            
        }

        ~RMQQueue()
        {
            Dispose(false);
        }
        
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

        public void Dispose()
        {
            Dispose(true);
        }

        protected void OpenConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connctionFactory.CreateConnection(_connectionName);
            }
        }

        protected void CloseConnection()
        {
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }

        protected void OpenChannel()
        {
            try
            {
                if (_channel == null || !_channel.IsOpen)
                {
                    _channel = _connection.CreateModel();

                    _channel.QueueDeclare(
                        queue: _settings.QueueName, 
                        durable: _settings.Durable,
                        exclusive: _settings.Exclusive,
                        autoDelete: _settings.AutoDelete,
                        arguments: _settings.Arguments);
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

        public virtual bool IsOpen()
        {
            return _channel != null && _channel.IsOpen;
        }

        public virtual void Open()
        {
            if (IsOpen())
            {
                return;
            }

            OpenConnection();
            OpenChannel();
        }

        public virtual void Close()
        {
            CloseChannel();
            CloseConnection();
        }
    }
}
