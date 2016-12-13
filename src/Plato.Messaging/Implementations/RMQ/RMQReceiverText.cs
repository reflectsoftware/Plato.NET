// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace Plato.Messaging.Implementations.RMQ
{
    public class RMQReceiverText : RMQReceiverBase, IRMQReceiverText
    {
        public RMQReceiverText(RMQSettings settings, IEnumerable<string> routingKeys = null) : base(settings, routingKeys)
        {
        }

        public RMQReceiverText(RMQSettings settings, string routingKey = "") : base(settings, new string[] { routingKey })
        {
        }

        public IMessageReceiveResult<string> Receive(int msecTimeout = Timeout.Infinite)
        {
            var deliveryArgs = _Receive(msecTimeout);
            return new RMQReceiverResultText(_connection, _channel, deliveryArgs, _settings.QueueSettings.QueueName);
        }
    }
}
