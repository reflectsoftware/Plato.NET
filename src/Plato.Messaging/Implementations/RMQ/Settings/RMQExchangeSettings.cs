// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class RMQExchangeSettings
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        /// <value>
        /// The name of the exchange.
        /// </value>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RMQExchangeSettings"/> is durable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if durable; otherwise, <c>false</c>.
        /// </value>
        public bool Durable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic delete]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public IDictionary<string, object> Arguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQExchangeSettings" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="type">The type.</param>
        /// <param name="durable">if set to <c>true</c> [durable].</param>
        /// <param name="autoDelete">if set to <c>true</c> [automatic delete].</param>
        /// <param name="arguments">The arguments.</param>
        public RMQExchangeSettings(string name, string exchangeName = "", string type = "direct", bool durable = true, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            Name = name;
            ExchangeName = exchangeName;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }
    }
}
