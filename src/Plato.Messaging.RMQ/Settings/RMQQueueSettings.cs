// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;
using System.Linq;

namespace Plato.Messaging.RMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class RMQQueueSettings
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RMQQueueSettings"/> is durable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if durable; otherwise, <c>false</c>.
        /// </value>
        public bool Durable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RMQQueueSettings" /> is exclusive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exclusive; otherwise, <c>false</c>.
        /// </value>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic delete]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RMQQueueSettings"/> is persistent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        public bool Persistent { get; set; }

        /// <summary>
        /// Gets or sets the routing keys.
        /// </summary>
        /// <value>
        /// The routing keys.
        /// </value>
        public IList<string> RoutingKeys { get; set; }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public IDictionary<string, object> Arguments { get; set; }

        /// <summary>
        /// Gets or sets the consumer settings.
        /// </summary>
        /// <value>
        /// The consumer settings.
        /// </value>
        public RMQConsumerSettings ConsumerSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQQueueSettings" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="durable">if set to <c>true</c> [durable].</param>
        /// <param name="exclusive">if set to <c>true</c> [exclusive].</param>
        /// <param name="autoDelete">if set to <c>true</c> [automatic delete].</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        /// <param name="arguments">The arguments.</param>
        public RMQQueueSettings(
            string name, 
            string queueName = null, 
            bool durable = true, 
            bool exclusive = false, 
            bool autoDelete = false,
            bool persistent = true,
            IList<string> routingKeys = null,
            IDictionary<string, object> arguments = null)
        {
            Name = name;
            QueueName = queueName ?? name;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Persistent = persistent;
            RoutingKeys = routingKeys ?? new List<string>();
            Arguments = arguments;
            ConsumerSettings = new RMQConsumerSettings();
        }
    }
}
