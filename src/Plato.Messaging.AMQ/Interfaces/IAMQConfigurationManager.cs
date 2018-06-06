// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Messaging.AMQ.Settings;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQConfigurationManager
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
        AMQConnectionSettings GetConnectionSettings(string name);


        /// <summary>
        /// Gets the destination settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        AMQDestinationSettings GetDestinationSettings(string name);
    }
}
