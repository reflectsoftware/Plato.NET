// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageReceiverSender" />
    public abstract class AMQReceiverSender : IMessageReceiverSender
    {        
        protected readonly AMQConnectionSettings _connectionSettings;
        protected readonly IAMQConnectionFactory _connectionFactory;        
        protected IConnection _connection;
        protected ISession _session;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IMessageReceiverSender" /> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQReceiverSender"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public AMQReceiverSender(IAMQConnectionFactory connectionFactory, AMQConnectionSettings connectionSettings)
        {
            Disposed = false;
            _connectionSettings = connectionSettings;
            _connectionFactory = connectionFactory;            
            _connection = null;
            _session = null;            
        }

        #region Dispose
        /// <summary>
        /// Finalizes an instance of the <see cref="AMQReceiverSender"/> class.
        /// </summary>
        ~AMQReceiverSender()
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
        #endregion Dispose

        /// <summary>
        /// Gets the delivery mode.
        /// </summary>
        /// <value>
        /// The delivery mode.
        /// </value>
        protected virtual AcknowledgementMode AcknowledgementMode
        {
            get { return AcknowledgementMode.AutoAcknowledge; }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        protected void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = _connectionFactory.CreateConnection(_connectionSettings);
            }

            if (_session == null)
            {
                _session = _connection.CreateSession(AcknowledgementMode);
            }

            _connection.Start();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        protected void CloseConnection()
        {
            if (_session != null)
            {
                _session.Close();
                _session.Dispose();
                _session = null;
            }

            if (_connection != null)
            {
                _connection.Stop();
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
      
        /// <summary>
        /// Determines whether this instance is open.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOpen()
        {
            return _connection != null && _connection.IsStarted;
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
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public virtual void Close()
        {            
            CloseConnection();
        }
    }
}
