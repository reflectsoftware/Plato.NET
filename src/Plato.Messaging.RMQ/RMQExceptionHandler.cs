// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    internal static class RMQExceptionHandler
    {
        /// <summary>
        /// Exceptions the handler.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static Exception ExceptionHandler(IConnection connection, Exception ex)
        {
            if (ex is NullReferenceException)
            {
                if (connection == null || !connection.IsOpen)
                {
                    return new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
                }
            }

            if (ex is OperationInterruptedException)
            {
                var opException = (ex as OperationInterruptedException);
                if (opException.ShutdownReason.ReplyCode == RabbitMQ.Client.Framing.Constants.ResourceLocked
                ||  opException.ShutdownReason.ReplyCode == RabbitMQ.Client.Framing.Constants.AccessRefused)
                {
                    return new MessageException(MessageExceptionCode.ExclusiveLock, ex.Message, ex);
                }
            }

            if (ex is AlreadyClosedException)
            {
                return new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
            }

            if (ex is IOException)
            {
                return new MessageException(MessageExceptionCode.LostConnection, ex.Message, ex);
            }

            return null;
        }
    }
}
