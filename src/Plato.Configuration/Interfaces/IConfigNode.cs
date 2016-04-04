// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace Plato.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigNode
    {
        /// <summary>
        /// Gets the configuration node.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        IConfigNode GetConfigNode(string xPath);

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        XmlNode GetNode(string xPath);

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        XmlNodeList GetNodes(string xPath);

        /// <summary>
        /// Gets the configuration nodes.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        List<IConfigNode> GetConfigNodes(string xPath);

        /// <summary>
        /// Gets the node inner text.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        string GetNodeInnerText(string xPath);

        /// <summary>
        /// Gets the node inner text.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        string GetNodeInnerText(string xPath, string defaultValue);

        /// <summary>
        /// Gets the node by name value.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="attrValue">The attribute value.</param>
        /// <returns></returns>
        XmlNode GetNodeByNameValue(string xPath, string attrName, string attrValue);

        /// <summary>
        /// Gets the configuration node by name value.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="attrValue">The attribute value.</param>
        /// <returns></returns>
        IConfigNode GetConfigNodeByNameValue(string xPath, string attrName, string attrValue);

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        NameValueCollection GetAttributes(string xPath = ".");

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attName">Name of the att.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        string GetAttribute(string xPath, string attName, string defaultValue);

        /// <summary>
        /// Gets the key value attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        string GetKeyValueAttribute(string xPath, string key, string defaultValue);

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attName">Name of the att.</param>
        /// <param name="obj">The object.</param>
        void SetAttribute(string xPath, string attName, object obj);

        /// <summary>
        /// Sets the node inner text.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="obj">The object.</param>
        void SetNodeInnerText(string xPath, object obj);

        /// <summary>
        /// Gets the x node.
        /// </summary>
        /// <value>
        /// The x node.
        /// </value>
        XmlNode XNode { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        XmlDocument Document { get; }

        /// <summary>
        /// Gets the XML string.
        /// </summary>
        /// <value>
        /// The XML string.
        /// </value>
        string XmlString { get; }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        XmlNode Section { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is section set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is section set; otherwise, <c>false</c>.
        /// </value>
        bool IsSectionSet  { get; }
    }
}
