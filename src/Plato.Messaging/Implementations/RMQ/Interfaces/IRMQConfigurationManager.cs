// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQConfigurationManager
    {
        /// <summary>
        /// Gets the attributes collection for all nodes.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        List<NameValueCollection> GetAttributesCollectionForAllNodes(string nodeName);

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        NameValueCollection GetAttributes(string nodeName, string name);

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="name">The name.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        string GetAttribute(string nodeName, string name, string attribute, string defaultValue = null);

        /// <summary>
        /// Gets the connection settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        RMQConnectionSettings GetConnectionSettings(string name);

        /// <summary>
        /// Gets the exchange settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        RMQExchangeSettings GetExchangeSettings(string name, IDictionary<string, object> arguments = null);

        /// <summary>
        /// Gets the queue settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        RMQQueueSettings GetQueueSettings(string name, IDictionary<string, object> arguments = null);
    }
}
