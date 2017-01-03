// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration;
using Plato.ExceptionManagement.Interfaces;

namespace Plato.ExceptionManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class PublisherInfo
    {
        /// <summary>
        /// Gets the publisher attributes.
        /// </summary>
        /// <value>
        /// The publisher attributes.
        /// </value>
        public NodeAttributes PublisherAttributes { get; internal set; }

        /// <summary>
        /// Gets the publisher.
        /// </summary>
        /// <value>
        /// The publisher.
        /// </value>
        public IExceptionPublisher Publisher { get; internal set; }
    }
}
