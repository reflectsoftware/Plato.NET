// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Specialized;

namespace Plato.ExceptionManagement.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExceptionPublisher
    {
        /// <summary>
        /// Publishes the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        void Publish(Exception ex, NameValueCollection additionalParameters);
    }
}
