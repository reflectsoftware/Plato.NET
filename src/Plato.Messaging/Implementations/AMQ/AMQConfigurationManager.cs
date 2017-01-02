// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;
using Plato.Configuration;
using Plato.Messaging.Implementations.AMQ.Interfaces;
using Plato.Messaging.Implementations.AMQ.Settings;
using Plato.Core.Strings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Plato.Messaging.Implementations.AMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Messaging.AMQ.Interfaces.IAMQConfigurationManager" />
    public class AMQConfigurationManager : SimpleConfigurationSectionManager, IAMQConfigurationManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AMQConfigurationManager"/> class.
        /// </summary>
        public AMQConfigurationManager() : base("amqSettings")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQConfigurationManager"/> class.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public AMQConfigurationManager(IEnumerable<AMQConnectionSettings> connections, IEnumerable<AMQDestinationSettings> destinations = null)
        {
            NodeAttributes = new NodeChildAttributes();
            NodeAttributes.ParentAttributes.NodeName = "amqSettings";

            if(connections != null)
            {
                foreach(var connection in connections)
                {
                    var childNode = new NodeAttributes()
                    {
                        NodeName = "connectionSettings",
                        Attributes = new NameValueCollection()
                    };

                    childNode.Attributes["name"] = connection.Name;
                    childNode.Attributes["username"] = connection.Username;
                    childNode.Attributes["password"] = connection.Password;
                    childNode.Attributes["uri"] = connection.Uri;                    
                    childNode.Attributes["asyncSend"] = connection.AsyncSend ? "true" : "false";
                    childNode.Attributes["delayOnReconnect"] = connection.DelayOnReconnect.ToString();

                    NodeAttributes.ChildAttributes.Add(childNode);
                }
            }
            
            if(destinations != null)
            {
                foreach (var destination in destinations)
                {
                    var childNode = new NodeAttributes()
                    {
                        NodeName = "destinationSettings",
                        Attributes = new NameValueCollection()
                    };

                    childNode.Attributes["name"] = destination.Name;
                    childNode.Attributes["selector"] = destination.Selector;
                    childNode.Attributes["subscriberId"] = destination.SubscriberId;
                    childNode.Attributes["path"] = destination.Path;
                    childNode.Attributes["deliveryMode"] = destination.DeliveryMode.ToString().ToLower();
                    childNode.Attributes["ackMode"] = destination.AckMode.ToString().ToLower();
                    childNode.Attributes["durable"] = destination.Durable ? "true" : "false";

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
            List<NameValueCollection> collections = null;

            if (NodeAttributes != null)
            {
                collections = NodeAttributes.ChildAttributes.Where(x => x.NodeName == nodeName).Select(x => x.Attributes).ToList();
            }

            return collections ?? new List<NameValueCollection>();
        }

        /// <summary>
        /// Gets the connection settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public AMQConnectionSettings GetConnectionSettings(string name)
        {
            var attributes = GetAttributes("connectionSettings", name);

            var delayOnReconnectValue = StringHelper.FullTrim(StringHelper.IfNullOrEmptyUseDefault(attributes["delayOnReconnect"], "0"));
            var delayOnReconnect = 0;
            if(!int.TryParse(delayOnReconnectValue, out delayOnReconnect) || delayOnReconnect < 0)
            {
                delayOnReconnect = 0;
            }

            var settings = new AMQConnectionSettings()
            {
                Name = StringHelper.IfNullOrEmptyUseDefault(attributes["name"], string.Empty),
                Uri = StringHelper.FullTrim(StringHelper.IfNullOrEmptyUseDefault(attributes["uri"], string.Empty)),
                Username = StringHelper.IfNullOrEmptyUseDefault(attributes["username"], "admin"),
                Password = StringHelper.IfNullOrEmptyUseDefault(attributes["password"], "admin"),
                AsyncSend = StringHelper.IfNullOrEmptyUseDefault(attributes["asyncSend"], "true") == "true",
                DelayOnReconnect = delayOnReconnect,
            };

            foreach (var uri in settings.Uri.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                settings.Endpoints.Add(uri);
            }

            return settings;
        }

        /// <summary>
        /// Gets the destination settings.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public AMQDestinationSettings GetDestinationSettings(string name)
        {
            var attributes = GetAttributes("destinationSettings", name);

            var destination = new AMQDestinationSettings(attributes["name"], attributes["path"])
            {
                Path = StringHelper.FullTrim(StringHelper.IfNullOrEmptyUseDefault(attributes["path"], string.Empty)),
                Selector = StringHelper.IfNullOrEmptyUseDefault(attributes["selector"], null),
                SubscriberId = StringHelper.IfNullOrEmptyUseDefault(attributes["subscriberId"], null),                
                Durable = StringHelper.IfNullOrEmptyUseDefault(attributes["durable"], "false") == "true",
                
                DeliveryMode = MsgDeliveryMode.Persistent,
                AckMode = AcknowledgementMode.AutoAcknowledge
            };
                        
            var deliveryMode = StringHelper.IfNullOrEmptyUseDefault(attributes["deliveryMode"], "persistent");
            MsgDeliveryMode msgDeliveryMode = MsgDeliveryMode.Persistent;
            Enum.TryParse(deliveryMode, true, out msgDeliveryMode);
            destination.DeliveryMode = msgDeliveryMode;
            
            var ackMode = StringHelper.IfNullOrEmptyUseDefault(attributes["ackMode"], "autoacknowledge");
            AcknowledgementMode msgAckMode = AcknowledgementMode.AutoAcknowledge;
            Enum.TryParse(ackMode, true, out msgAckMode);
            destination.AckMode = msgAckMode;
            
            return destination;
        }
    }
}
