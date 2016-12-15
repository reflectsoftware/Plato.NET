// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration;
using Plato.Configuration.Interfaces;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Utils.Strings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQConfigurationManager" />
    public class RMQConfigurationManager : IRMQConfigurationManager
    {
        private readonly NodeChildAttributes _nodeAttributes;
        private readonly IConfigNode _configNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConfigurationManager"/> class.
        /// </summary>
        public RMQConfigurationManager()
        {
            var xmlConfigSection = (XmlNode)ConfigurationManager.GetSection("rmqSettings");
            if (xmlConfigSection != null)
            {
                _configNode = new ConfigNode(xmlConfigSection);
                _nodeAttributes = ConfigHelper.GetNodeChildAttributes(_configNode, ".");
            }
        }

        /// <summary>
        /// Gets the attributes collection for all nodes.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        public List<NameValueCollection> GetAttributesCollectionForAllNodes(string nodeName)
        {
            List<NameValueCollection> collections = null;

            if (_nodeAttributes != null)
            {
                collections = _nodeAttributes.ChildAttributes.Where(x => x.NodeName == nodeName).Select(x => x.Attributes).ToList();
            }

            return collections ?? new List<NameValueCollection>();
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

        /// <summary>
        /// Gets the connection settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RMQConnectionSettings GetConnectionSettings(string name)
        {
            var attributes = GetAttributes("connectionSettings", name);

            return new RMQConnectionSettings()
            {
                HostName = StringHelper.IfNullOrEmptyUseDefault(attributes["hostname"], string.Empty),
                Username = StringHelper.IfNullOrEmptyUseDefault(attributes["username"], string.Empty),
                Password = StringHelper.IfNullOrEmptyUseDefault(attributes["password"], string.Empty),
                VirtualHost = StringHelper.IfNullOrEmptyUseDefault(attributes["virtualhost"], string.Empty),
                Port = int.Parse(StringHelper.IfNullOrEmptyUseDefault(attributes["port"], "5672")),
                Protocol = Protocols.DefaultProtocol
            };
        }

        /// <summary>
        /// Gets the exchange settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public RMQExchangeSettings GetExchangeSettings(string name, IDictionary<string, object> arguments = null)
        {
            var attributes = GetAttributes("exchange", name);

            return new RMQExchangeSettings(name)
            {
                ExchangeName = StringHelper.IfNullOrEmptyUseDefault(attributes["exchangeName"], ""),
                Type = StringHelper.IfNullOrEmptyUseDefault(attributes["type"], "direct"),
                Durable = StringHelper.IfNullOrEmptyUseDefault(attributes["durable"], "true") == "true",
                AutoDelete = StringHelper.IfNullOrEmptyUseDefault(attributes["autoDelete"], "false") == "true",
                Arguments = arguments
            };
        }

        /// <summary>
        /// Gets the queue settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public RMQQueueSettings GetQueueSettings(string name, IDictionary<string, object> arguments = null)
        {
            var attributes = GetAttributes("queue", name);
            var routingKeys = new List<string>();

            var queueSettings = new RMQQueueSettings(name)
            {
                QueueName = StringHelper.IfNullOrEmptyUseDefault(attributes["QueueName"], name),
                Exclusive = StringHelper.IfNullOrEmptyUseDefault(attributes["exclusive"], "true") == "true",
                Durable = StringHelper.IfNullOrEmptyUseDefault(attributes["durable"], "true") == "true",
                AutoDelete = StringHelper.IfNullOrEmptyUseDefault(attributes["autoDelete"], "false") == "true",
                Persistent = StringHelper.IfNullOrEmptyUseDefault(attributes["persistent"], "true") == "true",
                RoutingKeys = routingKeys,
                Arguments = arguments
            };

            var sRoutingKeys = StringHelper.IfNullOrEmptyUseDefault(attributes["routingKeys"], string.Empty);
            foreach(var routingKey in sRoutingKeys.Split(new char[] {','}))
            {
                routingKeys.Add(routingKey.Trim());
            }

            if(_configNode != null)
            {
                var consumerNode = _configNode.GetConfigNode($"./queue[@name='{name}']/consumer");
                if(consumerNode != null)
                {
                    attributes = consumerNode.GetAttributes();
                    queueSettings.ConsumerSettings.Tag = StringHelper.IfNullOrEmptyUseDefault(attributes["tag"], Guid.NewGuid().ToString());
                    queueSettings.ConsumerSettings.Exclusive = StringHelper.IfNullOrEmptyUseDefault(attributes["exclusive"], "true") == "true";
                    queueSettings.ConsumerSettings.NoAck = StringHelper.IfNullOrEmptyUseDefault(attributes["noAck"], "true") == "true";
                    queueSettings.ConsumerSettings.NoLocal = StringHelper.IfNullOrEmptyUseDefault(attributes["noLocal"], "true") == "true";
                }
            }
            
            return queueSettings;
        }
    }
}
