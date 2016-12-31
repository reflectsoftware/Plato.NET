// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    public abstract class RMQReceiverSender : IMessageReceiverSender
    {
        protected static TimeoutException _TimeoutException;

        protected readonly IRMQConnectionFactory _connctionFactory;
        protected readonly string _connectionName;
        protected IConnection _connection;
        protected IModel _channel;

        public bool Disposed { get; private set; }

        static RMQReceiverSender()
        {
            _TimeoutException = new TimeoutException();
        }
        
        public RMQReceiverSender(IRMQConnectionFactory connctionFactory, string connectionName)
        {
            Disposed = false;
            _connctionFactory = connctionFactory;
            _connectionName = connectionName;
            _channel = null;            
        }

        ~RMQReceiverSender()
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

        protected virtual void OpenConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connctionFactory.CreateConnection(_connectionName);
            }
        }

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
