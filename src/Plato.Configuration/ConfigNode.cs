// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;
using Plato.Core.Strings;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Configuration.Interfaces.IConfigNode" />
    public class ConfigNode : IConfigNode
    {
        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <value>
        /// The section.
        /// </value>
        public XmlNode Section { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigNode"/> class.
        /// </summary>
        /// <param name="section">The section.</param>
        public ConfigNode(XmlNode section = null)
        {
            Section = section;
        }

        /// <summary>
        /// Gets the configuration node.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        public IConfigNode GetConfigNode(string xPath)
        {            
            var node = GetNode(xPath);
            return node != null ? new ConfigNode(node) : null;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        public XmlNode GetNode(string xPath)
        {
            return Section?.SelectSingleNode(xPath);
        }

        /// <summary>
        /// Gets the x node.
        /// </summary>
        /// <value>
        /// The x node.
        /// </value>
        public XmlNode XNode
        {
            get
            {
                return GetNode(".");
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return XNode != null ? XNode.Name : null;
            }
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        public XmlNodeList GetNodes(string xPath)
        {
            return Section?.SelectNodes(xPath);
        }

        /// <summary>
        /// Gets the configuration nodes.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        public List<IConfigNode> GetConfigNodes(string xPath)
        {
            var cNodes = new List<IConfigNode>();
            var nodes = GetNodes(xPath);
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    cNodes.Add(new ConfigNode(node));
                }
            }

            return cNodes;
        }

        /// <summary>
        /// Gets the node inner text.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        public string GetNodeInnerText(string xPath)
        {
            var node = GetNode(xPath);

            return node != null ? ConfigManager.InterceptValue(node.InnerText) : null;
        }

        /// <summary>
        /// Gets the node inner text.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetNodeInnerText(string xPath, string defaultValue)
        {
            return StringHelper.IfNullOrEmptyUseDefault(GetNodeInnerText(xPath), defaultValue);
        }

        /// <summary>
        /// Gets the node by name value.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="attrValue">The attribute value.</param>
        /// <returns></returns>
        public XmlNode GetNodeByNameValue(string xPath, string attrName, string attrValue)
        {
            return GetNode(string.Format("{0}[@{1}='{2}']", xPath, attrName, attrValue));
        }

        /// <summary>
        /// Gets the configuration node by name value.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="attrValue">The attribute value.</param>
        /// <returns></returns>
        public IConfigNode GetConfigNodeByNameValue(string xPath, string attrName, string attrValue)
        {
            return GetConfigNode(string.Format("{0}[@{1}='{2}']", xPath, attrName, attrValue));
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <returns></returns>
        public NameValueCollection GetAttributes(string xPath = ".")
        {
            var attributes = new NameValueCollection();
            var node = GetNode(xPath);
            if (node != null)
            {
                foreach (XmlAttribute xAtt in node.Attributes)
                {
                    attributes.Add(xAtt.Name, ConfigManager.InterceptValue(xAtt.Value));
                }
            }

            return attributes;
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attName">Name of the att.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetAttribute(string xPath, string attName, string defaultValue)
        {
            var rValue = defaultValue;
            var objNode = GetNode(xPath);

            if (objNode != null)
            {
                if (objNode.Attributes[attName] != null)
                {
                    rValue = objNode.Attributes[attName].Value;
                }
            }

            return ConfigManager.InterceptValue(StringHelper.IfNullOrEmptyUseDefault(rValue, defaultValue));
        }

        /// <summary>
        /// Gets the key value attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetKeyValueAttribute(string xPath, string key, string defaultValue)
        {
            return GetAttribute(string.Format("{0}[@key='{1}']", xPath, key), "value", defaultValue);
        }

        public void SetAttribute(string xPath, string attName, object obj)
        {
            var objNode = GetNode(xPath);
            if (objNode != null)
            {
                if (objNode.Attributes[attName] == null)
                {
                    objNode.Attributes.Append(objNode.OwnerDocument.CreateAttribute(attName));
                }

                objNode.Attributes[attName].Value = obj.ToString();
            }
        }

        /// <summary>
        /// Sets the node inner text.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="obj">The object.</param>
        public void SetNodeInnerText(string xPath, object obj)
        {
            var node = GetNode(xPath);
            if (node != null)
            {
                node.InnerText = obj.ToString();
            }
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public XmlDocument Document
        {
            get
            {
                return Section?.OwnerDocument;
            }
        }

        /// <summary>
        /// Gets the XML string.
        /// </summary>
        /// <value>
        /// The XML string.
        /// </value>
        public string XmlString
        {
            get
            {
                return Document != null ? Document.OuterXml : null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is section set.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is section set; otherwise, <c>false</c>.
        /// </value>
        public bool IsSectionSet
        {
            get
            {
                return Section != null;
            }
        }
    }
}
