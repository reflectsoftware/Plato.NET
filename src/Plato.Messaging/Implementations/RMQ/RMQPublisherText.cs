// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Text;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQPublisherText : RMQPublisher, IRMQPublisherText
    {
        public RMQPublisherText(
            IRMQConnectionFactory connectionFactory,
            RMQConnectionSettings connectionSettings,
            RMQExchangeSettings exchangeSettings,
            RMQQueueSettings queueSettings = null)
            : base(connectionFactory, connectionSettings, exchangeSettings, queueSettings)
        {
        }

        public void Send(string text, Action<ISenderProperties> action = null)
        {
            var data = Encoding.UTF8.GetBytes(text);
            _Send(data, action);
        }
    }
}
