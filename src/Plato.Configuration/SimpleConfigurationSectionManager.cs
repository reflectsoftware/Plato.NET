// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Plato.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleConfigurationSectionManager : ISimpleConfigurationSectionManager
    {
        public NodeChildAttributes NodeAttributes { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XSimpleConfigurationSectionManager"/> class.
        /// </summary>
        public SimpleConfigurationSectionManager(string settings = null, string configPath = null)
        {
            if(settings == null)
            {
                return;
            }

            if (configPath == null)
            {
                var xmlConfigSection = (XmlNode)ConfigurationManager.GetSection(settings);
                if (xmlConfigSection != null)
                {
                    var cc = new ConfigNode(xmlConfigSection);
                    NodeAttributes = ConfigHelper.GetNodeChildAttributes(cc, ".");
                }
            }
            else
            {
                using (var configContainer = new ConfigContainer(configPath, "./rmqSettings"))
                {
                    var cc = configContainer.Node;
                    NodeAttributes = ConfigHelper.GetNodeChildAttributes(cc, ".");
                }
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
            if (NodeAttributes != null)
            {
                var nodeAttributes = NodeAttributes.ChildAttributes.FirstOrDefault(x => x.NodeName == nodeName && x.Attributes["name"] == name);
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

        /// <summary>
        /// Gets the attribute values.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public IEnumerable<string> GetAttributeValues(string nodeName, string attribute)
        {
            var values = NodeAttributes
                .ChildAttributes
                .Where(x => x.NodeName == nodeName && x.Attributes[attribute] != null)
                .Select(x => x.Attributes[attribute]);

            return values;
        }

        /// <summary>
        /// Gets the attribute values.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public IEnumerable<string> GetAttributeValues(string attribute)
        {
            var values = NodeAttributes
                .ChildAttributes
                .Where(x => x.Attributes[attribute] != null)
                .Select(x => x.Attributes[attribute]);

            return values;
        }
    }
}
