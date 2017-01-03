// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
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
        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        /// <exception cref="MessageException">
        /// There are no acceptable endpoints to connect too.
        /// or
        /// </exception>
        public IConnection CreateConnection(RMQConnectionSettings settings)
        {            
            if(settings.Endpoints.Count == 0)
            {
                throw new MessageException(MessageExceptionCode.NoAcceptableEndpoints, "There are no acceptable endpoints to connect any RabbitMQ Broker.");
            }

            var exceptionList = new List<Exception>();
            var retries = settings.Endpoints.Count - 1;                       

            while(true)
            {
                try
                {
                    var connectionFactory = new ConnectionFactory()
                    {
                        UserName = settings.Username,
                        Password = settings.Password,
                        VirtualHost = settings.VirtualHost,
                        Protocol = settings.Protocol,
                        Uri = settings.Endpoints[settings.ActiveEndpointIndex]
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

                retries--;
                if(retries < 0)
                {
                    var finalException = new AggregateException($"Unable to connect to any RabbitMQ Broker using the following connection uri: {settings.Uri}, for named connection settings: '{settings.Name}'.", exceptionList);
                    throw new MessageException(MessageExceptionCode.LostConnection, finalException.Message, finalException);
                }

                settings.ActiveEndpointIndex++;
                if(settings.ActiveEndpointIndex >= settings.Endpoints.Count)
                {
                    settings.ActiveEndpointIndex = 0;
                }

                Thread.Sleep(settings.DelayOnReconnect);
            }
        }
    }
}
