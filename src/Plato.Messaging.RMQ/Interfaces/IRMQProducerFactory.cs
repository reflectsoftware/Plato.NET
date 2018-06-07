// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Settings;

namespace Plato.Messaging.RMQ.Interfaces
{
    public interface IRMQProducerFactory
    {
        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        IRMQProducerBytes CreateByte(RMQConnectionSettings connectionSettings, RMQQueueSettings queueSettings);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        IRMQProducerText CreateText(RMQConnectionSettings connectionSettings, RMQQueueSettings queueSettings);
    }
}
