// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Settings;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRMQSubscriberFactory
    {
        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        IRMQSubscriberByte CreateByte(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        IRMQSubscriberText CreateText(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings);
    }
}
