// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Settings;

namespace Plato.Messaging.RMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRMQSubscriberFactory
    {
        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <param name="queueSettings">The queue settings.</param>
        /// <returns></returns>
        IRMQSubscriberBytes CreateBytes(RMQConnectionSettings connectionSettings, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <returns></returns>
        IRMQSubscriberText CreateText(RMQConnectionSettings connectionSettings, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings);
    }
}
