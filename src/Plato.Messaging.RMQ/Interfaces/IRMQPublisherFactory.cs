// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.RMQ.Settings;

namespace Plato.Messaging.RMQ.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRMQPublisherFactory
    {
        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <returns></returns>
        IRMQPublisherBytes CreateBytes(RMQConnectionSettings connectionSettings, RMQExchangeSettings exchangeSettings);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <param name="exchangeSettings">The exchange settings.</param>
        /// <returns></returns>
        IRMQPublisherText CreateText(RMQConnectionSettings connectionSettings, RMQExchangeSettings exchangeSettings);        
    }
}
