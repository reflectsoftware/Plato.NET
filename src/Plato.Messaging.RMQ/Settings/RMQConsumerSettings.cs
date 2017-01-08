// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;

namespace Plato.Messaging.RMQ.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class RMQConsumerSettings
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RMQConsumerSettings"/> is exclusive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exclusive; otherwise, <c>false</c>.
        /// </value>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [no ack].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no ack]; otherwise, <c>false</c>.
        /// </value>
        public bool NoAck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [no local].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no local]; otherwise, <c>false</c>.
        /// </value>
        public bool NoLocal { get; set; }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public IDictionary<string, object> Arguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RMQConsumerSettings"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="exclusive">if set to <c>true</c> [exclusive].</param>
        /// <param name="noAck">if set to <c>true</c> [no ack].</param>
        /// <param name="noLocal">if set to <c>true</c> [no local].</param>
        /// <param name="arguments">The arguments.</param>
        public RMQConsumerSettings(string tag = null, bool exclusive = false, bool noAck = false, bool noLocal = false, IDictionary<string, object> arguments = null)
        {
            Tag = tag ?? Guid.NewGuid().ToString();
            Exclusive = exclusive;
            NoAck = noAck;
            NoLocal = noLocal;
            Arguments = arguments;
        }
    }
}
