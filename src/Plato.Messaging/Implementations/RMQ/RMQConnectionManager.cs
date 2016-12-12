// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

// TODO: Revisit this implementation. IConnection factory doesn't look right

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQConnectionManager" />
    public class RMQConnectionManager : IRMQConnectionManager
    {
        private class NamedConnection : IDisposable
        {
            public bool Disposed { get; set; }
            public int ReferenceCount { get; set; }
            public IConnection Connection { get; set; }
            
            public void Dispose()
            {
                lock (this)
                {
                    if (!Disposed)
                    {
                        Disposed = true;
                        GC.SuppressFinalize(this);

                        if (Connection == null)
                        {
                            return;
                        }

                        if (Connection.IsOpen)
                        {
                            Connection.Close();
                            Connection.Dispose();
                        }

                        Connection = null;
                    }
                }
            }
        }

        private Dictionary<string, NamedConnection> _connections;
        private readonly IRMQConfigurationManager _configManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConnectionManager"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public RMQConnectionManager(IRMQConfigurationManager configManager)
        {
            _connections = new Dictionary<string, NamedConnection>();
            _configManager = configManager;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RMQConnectionManager"/> class.
        /// </summary>
        ~RMQConnectionManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_connections)
            {
                foreach (var namedConnection in _connections.Values)
                {
                    DisposedConnection(namedConnection);
                }

                _connections.Clear();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void DisposedConnection(NamedConnection namedConnection)
        {
            namedConnection.Connection.ConnectionShutdown -= ConnectionShutdown;
            namedConnection.Dispose();
        }

        /// <summary>
        /// Removes the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="namedConnection">The named connection.</param>
        private void RemoveConnection(string name, NamedConnection namedConnection)
        {
            lock (_connections)
            {
                _connections.Remove(name);
                DisposedConnection(namedConnection);
            }
        }

        /// <summary>
        /// Connections the shutdown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="se">The <see cref="ShutdownEventArgs"/> instance containing the event data.</param>
        private void ConnectionShutdown(object sender, ShutdownEventArgs se)
        {
            lock (_connections)
            {
                string name = null;
                NamedConnection namedConnection = null;

                foreach (KeyValuePair<string, NamedConnection> keyValue in _connections)
                {
                    if (ReferenceEquals(keyValue.Value.Connection, sender))
                    {
                        name = keyValue.Key;
                        namedConnection = keyValue.Value;
                        break;
                    }
                }

                if (namedConnection != null)
                {
                    RemoveConnection(name, namedConnection);
                }
            }
        }

        /// <summary>
        /// Declares the connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="MessageException">
        /// </exception>
        public IConnection DeclareConnection(string name)
        {
            lock (_connections)
            {
                IConnection connection;
                if (_connections.ContainsKey(name))
                {
                    _connections[name].ReferenceCount++;
                    connection = _connections[name].Connection;
                }
                else
                {
                    try
                    {
                        var connectionSettings = _configManager.GetConnectionSettings(name);
                        if (connectionSettings == null)
                        {
                            throw new KeyNotFoundException($"Unable to fined RMQ connection settings for {name}.");
                        }

                        var connectionFactory = new ConnectionFactory()
                        {
                            HostName = connectionSettings.HostName,
                            UserName = connectionSettings.Username,
                            Password = connectionSettings.Password,
                            VirtualHost = connectionSettings.VirtualHost,
                            Protocol = connectionSettings.Protocol,
                            Port = connectionSettings.Port
                        };
                        
                        connection = connectionFactory.CreateConnection();
                        connection.ConnectionShutdown += ConnectionShutdown;

                        var namedConnection = new NamedConnection()
                        {
                            ReferenceCount = 1,
                            Connection = connection,
                        };

                        _connections.Add(name, namedConnection);
                    }
                    catch (BrokerUnreachableException ex)
                    {
                        throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
                    }
                    catch (IOException ex)
                    {
                        throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
                    }
                }

                return connection;
            }
        }

        /// <summary>
        /// Removes the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveConnection(string name)
        {
            lock (_connections)
            {
                if (!_connections.ContainsKey(name))
                {
                    return;
                }

                var namedConnection = _connections[name];
                if (--namedConnection.ReferenceCount == 0)
                {
                    RemoveConnection(name, namedConnection);
                }
            }
        }
    }
}
