// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using System;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.RMQReceiverSender" />
    /// <seealso cref="Plato.Messaging.Interfaces.IMessageRPCResponder" />
    public class RMQRPCResponder : RMQReceiverSender, IMessageRPCResponder<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RMQRPCResponder"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RMQRPCResponder(RMQSettings settings): base(settings)
        {
        }

        public void Respond(IMessageReceiveResult<byte[]> result, byte[] data)
        {
            if (!IsOpen())
            {
                Open();
            }

            try
            {
                var receiverResults = (result as RMQReceiverResult);
                var body = data;
                var props = receiverResults.DeliverEventArgs.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();

                replyProps.CorrelationId = props.CorrelationId;

                _channel.BasicPublish(string.Empty, props.ReplyTo, replyProps, body);
                receiverResults.Acknowledge();
            }
            catch (Exception ex)
            {
                var newException = RMQExceptionHandler.ExceptionHandler(_connection, ex);
                if (newException != null)
                {
                    throw newException;
                }

                throw;
            }
        }
    }
}
