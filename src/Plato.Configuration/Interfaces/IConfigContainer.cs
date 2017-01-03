// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Configuration.Enums;
using System;

namespace Plato.Configuration.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IConfigContainer : IDisposable
    {
        /// <summary>
        /// Occurs when [on configuration change].
        /// </summary>
        event Action<IConfigContainer, OnChangeType> OnConfigChange;

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Watches the allow on change attribute.
        /// </summary>
        /// <param name="xPath">The x path.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <param name="value">The value.</param>
        void WatchAllowOnChangeAttribute(string xPath, string attrName, string value);

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        IConfigNode Node { get; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        IConfigSettings Settings { get; }

        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        /// <value>
        /// The connection strings.
        /// </value>
        IConfigSettings ConnectionStrings { get; }
    }
}
