﻿// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration;
using Plato.Configuration.Interfaces;
using Plato.Messaging.RMQ.Interfaces;
using Plato.Messaging.RMQ.Settings;
using Plato.Core.Strings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Text;

namespace Plato.Messaging.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.RMQ.Interfaces.IRMQConfigurationManager" />
    public class RMQConfigurationManager : IRMQConfigurationManager
    {        
        private readonly IConfigNode _configNode;

        public NodeChildAttributes NodeAttributes { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConfigurationManager"/> class.
        /// </summary>
        /// <param name="configPath">The configuration path.</param>
        public RMQConfigurationManager(string configPath = null)
        {
            if (configPath == null)
            {
                try
                {
                    var xmlConfigSection = (XmlNode)ConfigurationManager.GetSection("rmqSettings");
                    if (xmlConfigSection != null)
                    {
                        _configNode = new ConfigNode(xmlConfigSection);
                        NodeAttributes = ConfigHelper.GetNodeChildAttributes(_configNode, ".");
                    }
                }
                catch (ConfigurationErrorsException)
                {
                    NodeAttributes = new NodeChildAttributes();
                }
            }
            else
            {
                using (var configContainer = new ConfigContainer(configPath, "./rmqSettings"))
                {
                    _configNode = configContainer.Node;
                    NodeAttributes = ConfigHelper.GetNodeChildAttributes(_configNode, ".");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConfigurationManager" /> class.
        /// </summary>
        /// <param name="connections">The connections.</param>
        /// <param name="queueSettings">The queue settings.</param>
        public RMQConfigurationManager(
            IEnumerable<RMQConnectionSettings> connections, 
            IEnumerable<RMQExchangeSettings> exchangeSettings = null,
            IEnumerable<RMQQueueSettings> queueSettings = null)
        {
            NodeAttributes = new NodeChildAttributes();
            NodeAttributes.ParentAttributes.NodeName = "rmqSettings";

            if (connections != null)
            {
                foreach (var connection in connections)
                {
                    var childNode = new NodeAttributes()
                    {
                        NodeName = "connectionSettings",
                        Attributes = new NameValueCollection()
                    };

                    childNode.Attributes["name"] = connection.Name;
                    childNode.Attributes["username"] = connection.Username;
                    childNode.Attributes["password"] = connection.Password;
                    childNode.Attributes["virtualhost"] = connection.VirtualHost;
                    childNode.Attributes["uri"] = connection.Uri;
                    childNode.Attributes["delayOnReconnect"] = connection.DelayOnReconnect.ToString();

                    NodeAttributes.ChildAttributes.Add(childNode);
                }
            }

            if(exchangeSettings != null)
            {
                foreach (var exchangeSetting in exchangeSettings)
                {
                    var childNode = new NodeAttributes()
                    {
                        NodeName = "exchange",
                        Attributes = new NameValueCollection()
                    };

                    childNode.Attributes["name"] = exchangeSetting.Name;
                    childNode.Attributes["exchangeName"] = exchangeSetting.ExchangeName;
                    childNode.Attributes["type"] = exchangeSetting.Type;
                    childNode.Attributes["autoDelete"] = exchangeSetting.AutoDelete ? "true" : "false";                    

                    NodeAttributes.ChildAttributes.Add(childNode);
                }
            }

            if (queueSettings != null)
            {
                foreach (var queueSetting in queueSettings)
                {
                    var childNode = new NodeAttributes()
                    {
                        NodeName = "queue",
                        Attributes = new NameValueCollection()
                    };

                    childNode.Attributes["name"] = queueSetting.Name;
                    childNode.Attributes["queueName"] = queueSetting.QueueName;                    
                    childNode.Attributes["durable"] = queueSetting.Durable ? "true" : "false";
                    childNode.Attributes["exclusive"] = queueSetting.Exclusive ? "true" : "false";
                    childNode.Attributes["autoDelete"] = queueSetting.AutoDelete ? "true" : "false";
                    childNode.Attributes["persistent"] = queueSetting.Persistent ? "true" : "false";   
                    
                    if(queueSetting.RoutingKeys != null && queueSetting.RoutingKeys.Any())
                    {
                        var sb = new StringBuilder();
                        foreach(var rkey in queueSetting.RoutingKeys)
                        {
                            sb.Append($"{rkey},");
                        }

                        // remove the extra comma
                        sb.Remove(sb.Length - 1, 1);
                        childNode.Attributes["routingKeys"] = sb.ToString();
                    }                    

                    NodeAttributes.ChildAttributes.Add(childNode);
                }
            }
        }

        /// <summary>
        /// Gets the attributes collection for all nodes.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns></returns>
        public List<NameValueCollection> GetAttributesCollectionForAllNodes(string nodeName)
        {
            var collections = (List<NameValueCollection>)null;

            if (NodeAttributes != null)
            {
                collections = NodeAttributes.ChildAttributes.Where(x => x.NodeName == nodeName).Select(x => x.Attributes).ToList();
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
            var attributes = (NameValueCollection)null;
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
        /// Gets the connection settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public RMQConnectionSettings GetConnectionSettings(string name)
        {
            var attributes = GetAttributes("connectionSettings", name);

            var settings = new RMQConnectionSettings()
            {
                Protocol = Protocols.DefaultProtocol,
                Name = StringHelper.IfNullOrEmptyUseDefault(attributes["name"], string.Empty),
                Username = StringHelper.IfNullOrEmptyUseDefault(attributes["username"], string.Empty),
                Password = StringHelper.IfNullOrEmptyUseDefault(attributes["password"], string.Empty),
                VirtualHost = StringHelper.IfNullOrEmptyUseDefault(attributes["virtualhost"], string.Empty),
                DelayOnReconnect = int.Parse(StringHelper.IfNullOrEmptyUseDefault(attributes["delayOnReconnect"], "1000")),
                Uri = StringHelper.IfNullOrEmptyUseDefault(attributes["uri"], "amqp://localhost:5672"),
                ForceReconnectionTime = TimeSpan.FromMinutes(int.Parse(StringHelper.IfNullOrEmptyUseDefault(attributes["forceReconnectionTime"], "0")))
            };

            foreach (var uri in settings.Uri.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                settings.Endpoints.Add(uri);
            }

            return settings;
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
                QueueName = StringHelper.IfNullOrEmptyUseDefault(attributes["queueName"], name),
                Exclusive = StringHelper.IfNullOrEmptyUseDefault(attributes["exclusive"], "true") == "true",
                Durable = StringHelper.IfNullOrEmptyUseDefault(attributes["durable"], "true") == "true",
                AutoDelete = StringHelper.IfNullOrEmptyUseDefault(attributes["autoDelete"], "false") == "true",
                Persistent = StringHelper.IfNullOrEmptyUseDefault(attributes["persistent"], "true") == "true",
                RoutingKeys = routingKeys,
                Arguments = arguments ?? new Dictionary<string, object>()
            };

            var sRoutingKeys = StringHelper.IfNullOrEmptyUseDefault(attributes["routingKeys"], string.Empty);
            foreach (var routingKey in sRoutingKeys.Split(new char[] { ',' }))
            {
                routingKeys.Add(routingKey.Trim());
            }

            if (_configNode != null)
            {
                // get arguments if they exist
                var argsNode = _configNode.GetConfigNode($"./queue[@name='{name}']/arguments");
                if (argsNode != null)
                {
                    attributes = argsNode.GetAttributes();
                    foreach (var key in attributes.AllKeys)
                    {
                        if (!queueSettings.Arguments.ContainsKey(key))
                        {
                            queueSettings.Arguments[key] = attributes[key];
                        }
                    }
                }

                // get consumer info if it exists
                var consumerNode = _configNode.GetConfigNode($"./queue[@name='{name}']/consumer");
                if (consumerNode != null)
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
