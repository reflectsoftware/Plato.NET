// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Messaging.Implementations.AMQ.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.Implementations.AMQ.Interfaces.IAMQConnectionFactory" />
    public class AMQConnectionFactory : IAMQConnectionFactory
    {
        /// <summary>
        /// Declares the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="MessageException">
        /// </exception>
        public IConnection CreateConnection(AMQConnectionSettings settings)
        {
            if (settings.Endpoints.Count == 0)
            {
                throw new MessageException(MessageExceptionCode.NoAcceptableEndpoints, "There are no acceptable endpoints to connect any ActiveMQ Broker.");
            }

            var exceptionList = new List<Exception>();
            var retries = settings.Endpoints.Count - 1;

            while (true)
            {
                try
                {
                    var connectionUri = new Uri(settings.Endpoints[settings.ActiveEndpointIndex]);
                    var connectionFactory = new ConnectionFactory(connectionUri)
                    {
                        UserName = settings.Username,
                        Password = settings.Password,
                        AsyncSend = settings.AsyncSend,
                    };

                    var connection = connectionFactory.CreateConnection();
                    return connection;
                }
                catch (NMSConnectionException ex)
                {
                    exceptionList.Add(ex);
                }
                catch (InvalidClientIDException ex)
                {
                    exceptionList.Add(ex);
                }
                catch (NMSException ex)
                {
                    exceptionList.Add(ex);
                }
                catch (UriFormatException ex)
                {
                    exceptionList.Add(ex);
                }

                retries--;
                if (retries < 0)
                {
                    var finalException = new AggregateException($"Unable to connect to any ActiveMQ Broker using the following connection uri: {settings.Uri}, for named connection settings: '{settings.Name}'.", exceptionList);
                    throw new MessageException(MessageExceptionCode.LostConnection, finalException.Message, finalException);
                }

                settings.ActiveEndpointIndex++;
                if (settings.ActiveEndpointIndex >= settings.Endpoints.Count)
                {
                    settings.ActiveEndpointIndex = 0;
                }

                Thread.Sleep(settings.DelayOnReconnect);
            }
        }
    }
}
