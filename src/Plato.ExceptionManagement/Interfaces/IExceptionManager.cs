// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Specialized;

namespace Plato.ExceptionManagement.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IExceptionManager : IDisposable
    {
        /// <summary>
        /// Gets the publisher infos.
        /// </summary>
        /// <value>
        /// The publisher infos.
        /// </value>
        PublisherInfo[] PublisherInfos { get; }

        /// <summary>
        /// Publishes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        void Publish(Exception ex, NameValueCollection additionalParameters);

        /// <summary>
        /// Publishes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        void Publish(Exception ex);

        /// <summary>
        /// Adds the publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="parameters">The parameters.</param>
        void AddPublisher(IExceptionPublisher publisher, NameValueCollection parameters);

        /// <summary>
        /// Adds the publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        void AddPublisher(IExceptionPublisher publisher);

        /// <summary>
        /// Gets the publisher count.
        /// </summary>
        /// <value>
        /// The publisher count.
        /// </value>
        int PublisherCount { get; }
    }
}
