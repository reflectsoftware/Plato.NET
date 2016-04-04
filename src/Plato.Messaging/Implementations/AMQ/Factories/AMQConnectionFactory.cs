// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using System;

namespace Plato.Messaging.Implementations.AMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.Interfaces.IAMQConnectionFactory" />
    public class AMQConnectionFactory : IAMQConnectionFactory
    {
        private IAMQConfigurationManager _configManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQConnectionFactory"/> class.
        /// </summary>
        /// <param name="connectionSettings">The AMQ connection settings.</param>
        public AMQConnectionFactory(IAMQConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        /// <summary>
        /// Declares the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="MessageException">
        /// </exception>
        public IConnection CreateConnection(string name)
        {
            try
            {
                var connectionSettings = _configManager.GetConnectionSettings(name);
                var connectionUri = new Uri(connectionSettings.Uri);
                var connectionFactory = new ConnectionFactory(connectionUri)
                {
                    UserName = connectionSettings.Username,
                    Password = connectionSettings.Password,
                    AsyncSend = connectionSettings.AsyncSend,
                };

                var connection = connectionFactory.CreateConnection();
                return connection;
            }
            catch (NMSConnectionException ex)
            {
                throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
            }
            catch (InvalidClientIDException ex)
            {
                throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
            }
            catch (NMSException ex)
            {
                throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
            }
            catch (UriFormatException ex)
            {
                throw new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
            }
        }
    }
}
