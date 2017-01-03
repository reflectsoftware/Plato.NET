// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Collections.Specialized;

namespace Plato.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfigMultiSettings
    {
        string[] Keys { get; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        NameValueCollection[] Values { get;  }

        /// <summary>
        /// Gets the <see cref="NameValueCollection"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="NameValueCollection"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        NameValueCollection this[string key] { get;  }
    }
}
