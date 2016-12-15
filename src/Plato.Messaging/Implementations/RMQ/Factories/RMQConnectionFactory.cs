// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace Plato.Messaging.Implementations.RMQ.Factories
{
    public class RMQConnectionFactory : IRMQConnectionFactory
    {
        private IRMQConfigurationManager _configManager;

        public RMQConnectionFactory(IRMQConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        public IConnection CreateConnection(string name)
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

                var connection = connectionFactory.CreateConnection();
                return connection;
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
    }
}
