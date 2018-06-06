// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using System;

namespace Plato.Messaging.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    internal static class AMQExceptionHandler
    {
        /// <summary>
        /// Exceptions the handler.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static MessageException ExceptionHandler(IConnection connection, Exception ex)
        {
            if (ex is MessageException)
            {
                return ex as MessageException;
            }

            var eMsg = $"Unable to connect, or the connection to the ActiveMQ Broker is no longer available for the following reason: '{ex.Message}'";

            if (ex is NullReferenceException)
            {
                if (connection == null)
                {
                    return new MessageException(MessageExceptionCode.LostConnection, eMsg, ex);
                }
            }

            if (ex is System.IO.IOException 
            || ex is NMSConnectionException
            || ex is ConnectionClosedException
            || ex is BrokerException
            || ex is IOException
            || ex is InvalidClientIDException
            || ex is NMSException)
            {
                return new MessageException(MessageExceptionCode.LostConnection, eMsg, ex);
            }

            return null;
        }
    }
}
