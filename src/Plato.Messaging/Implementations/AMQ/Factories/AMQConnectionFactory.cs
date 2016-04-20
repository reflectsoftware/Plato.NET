// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using System;
using System.Collections.Generic;

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
            var connectionSettings = _configManager.GetConnectionSettings(name);

            if(string.IsNullOrWhiteSpace(connectionSettings.Uri))
            {
                throw new MessageException(MessageExceptionCode.UnhandledError, $"Missing or empty connection Uri for named connection settings: '{name}'.");
            }

            foreach (var uri in connectionSettings.Uri.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    var connectionUri = new Uri(uri);
                    var connectionFactory = new ConnectionFactory(connectionUri)
                    {
                        UserName = connectionSettings.Username,
                        Password = connectionSettings.Password,
                        AsyncSend = connectionSettings.AsyncSend,
                    };

                    var connection = connectionFactory.CreateConnection();
                    return connection;                    
                }
                catch (NMSConnectionException)
                {
                    // swallow
                }
                catch (InvalidClientIDException)
                {
                    // swallow
                }
                catch (NMSException)
                {
                    // swallow
                }
                catch (UriFormatException ex)
                {
                    throw new MessageException(MessageExceptionCode.UnhandledError, ex.Message, ex);
                }
            }

            throw new MessageException(MessageExceptionCode.LostConnection, $"Unable to connect to any ActiveMQ Broker using the following connection uri: {connectionSettings.Uri}, for named connection settings: '{name}'.");
        }
    }
}
