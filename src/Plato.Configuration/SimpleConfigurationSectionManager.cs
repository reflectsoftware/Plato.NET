// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleConfigurationSectionManager
    {
        private readonly NodeChildAttributes _nodeAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConfigurationSectionManager"/> class.
        /// </summary>
        public SimpleConfigurationSectionManager()
        {
        }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConfigurationSectionManager"/> class.
        /// </summary>
        public SimpleConfigurationSectionManager(string settings)
        {
            var xmlConfigSection = (XmlNode)ConfigurationManager.GetSection(settings);
            if (xmlConfigSection != null)
            {
                var cc = new ConfigNode(xmlConfigSection);
                _nodeAttributes = ConfigHelper.GetNodeChildAttributes(cc, ".");
            }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public NameValueCollection GetAttributes(string nodeName, string name)
        {
            NameValueCollection attributes = null;
            if (_nodeAttributes != null)
            {
                var nodeAttributes = _nodeAttributes.ChildAttributes.FirstOrDefault(x => x.NodeName == nodeName && x.Attributes["name"] == name);
                if (nodeAttributes != null)
                {
                    attributes = new NameValueCollection(nodeAttributes.Attributes);
                }
            }

            return attributes ?? new NameValueCollection();
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="name">The name.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetAttribute(string nodeName, string name, string attribute, string defaultValue = null)
        {
            var attributes = GetAttributes(nodeName, name);
            return attributes[attribute] ?? defaultValue;
        }
    }
}
