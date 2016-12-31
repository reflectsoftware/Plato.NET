// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.RMQ.Interfaces.IRMQConnectionFactory" />
    public class RMQConnectionFactory : IRMQConnectionFactory
    {
        private readonly IRMQConfigurationManager _configManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConnectionFactory"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public RMQConnectionFactory(IRMQConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="MessageException"></exception>
        public IConnection CreateConnection(string name)
        {
            var exceptionList = new List<Exception>();
            var connectionSettings = _configManager.GetConnectionSettings(name);
            if (string.IsNullOrWhiteSpace(connectionSettings.Uri))
            {
                exceptionList.Add(new MessageException(MessageExceptionCode.UnhandledError, $"Missing or empty RMQ connection Uri for named connection settings: '{name}'."));
            }

            foreach (var uri in connectionSettings.Uri.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    var connectionFactory = new ConnectionFactory()
                    {
                        UserName = connectionSettings.Username,
                        Password = connectionSettings.Password,
                        VirtualHost = connectionSettings.VirtualHost,
                        Protocol = connectionSettings.Protocol,
                        Uri = uri
                    };

                    var connection = connectionFactory.CreateConnection();
                    return connection;
                }
                catch (BrokerUnreachableException ex)
                {
                    exceptionList.Add(new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex));
                }
                catch (IOException ex)
                {
                    exceptionList.Add(new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex));
                }
                catch (UriFormatException ex)
                {
                    exceptionList.Add(ex);
                }

                Thread.Sleep(connectionSettings.DelayOnReconnect);
            }

            var finalException = new AggregateException($"Unable to connect to any RabbitMQ Broker using the following connection uri: {connectionSettings.Uri}, for named connection settings: '{name}'.", exceptionList);
            throw new MessageException(MessageExceptionCode.LostConnection, finalException.Message, finalException);
        }
    }
}
