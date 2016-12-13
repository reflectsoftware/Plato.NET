// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;
using System.Collections.Specialized;

namespace Plato.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimpleConfigurationSectionManager
    {
        /// <summary>
        /// Gets the node attributes.
        /// </summary>
        /// <value>
        /// The node attributes.
        /// </value>
        NodeChildAttributes NodeAttributes { get; }

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
        /// Gets the attributes.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        NameValueCollection GetAttributes(string nodeName, string name);

        /// <summary>
        /// Gets the attribute values.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        IEnumerable<string> GetAttributeValues(string nodeName, string attribute);

        /// <summary>
        /// Gets the attribute values.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        IEnumerable<string> GetAttributeValues(string attribute);
    }
}