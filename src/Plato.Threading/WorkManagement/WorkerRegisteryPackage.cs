// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Utils.Strings;
using System;
using System.Collections.Specialized;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    internal class WorkerRegisteryPackage
    {
        private readonly NameValueCollection _parameters;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="WorkerRegisteryPackage"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerRegisteryPackage"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <param name="parameters">The parameters.</param>
        public WorkerRegisteryPackage(string name, Type type, bool active, NameValueCollection parameters)
        {
            Type = type;
            Active = active;
            Name = name;
            _parameters = parameters ?? new NameValueCollection();
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns></returns>
        public NameValueCollection GetParameters()
        {
            return new NameValueCollection(_parameters);
        }

        /// <summary>
        /// Gets the parameter by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string Parameters(string key, string defaultValue = null)
        {
            return StringHelper.IfNullOrEmptyUseDefault(_parameters[key], defaultValue);
        }
    }
}
