// Plato.NET
// Copyright (c) 2018 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Apache.NMS;

namespace Plato.Messaging.AMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class AMQDestinationSettings
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AMQDestinationSettings"/> is durable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if durable; otherwise, <c>false</c>.
        /// </value>
        public bool Durable { get; set; }

        /// <summary>
        /// Gets or sets the delivery mode.
        /// </summary>
        /// <value>
        /// The delivery mode.
        /// </value>
        public MsgDeliveryMode DeliveryMode { get; set; }
        
        /// <summary>
        /// Gets or sets the acknowledgment mode.
        /// </summary>
        /// <value>
        /// The acknowledgment mode.
        /// </value>
        public AcknowledgementMode AckMode { get; set; }

        /// <summary>
        /// Gets or sets the selector.
        /// </summary>
        /// <value>
        /// The selector.
        /// </value>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the subscriber identifier.
        /// </summary>
        /// <value>
        /// The subscriber identifier.
        /// </value>
        public string SubscriberId { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AMQDestinationSettings" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        public AMQDestinationSettings(string name, string path)
        {
            Name = name;
            Path = path;
            DeliveryMode = MsgDeliveryMode.Persistent;
        }
    }
}
