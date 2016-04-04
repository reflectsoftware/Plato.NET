// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Interfaces;
using Plato.Utils.Strings;
using System.Collections.Specialized;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Threading.Interfaces.IWorkPackage" />
    public class WorkPackage : IWorkPackage
    {
        private readonly NameValueCollection _parameters;
        private readonly object _data;

        /// <summary>
        /// Gets or sets the watcher.
        /// </summary>
        /// <value>
        /// The watcher.
        /// </value>
        internal ThreadWatcher Watcher { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name instance.
        /// </summary>
        /// <value>
        /// The name instance.
        /// </value>
        public string NameInstance { get; private set; }

        /// <summary>
        /// Gets the work manager.
        /// </summary>
        /// <value>
        /// The work manager.
        /// </value>
        public WorkManager WorkManager { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="WorkPackage"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="nameInstance">The name instance.</param>
        /// <param name="workManager">The work manager.</param>
        /// <param name="watcher">The watcher.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="data">The data.</param>
        internal WorkPackage(string name, string nameInstance, WorkManager workManager, ThreadWatcher watcher, NameValueCollection parameters, object data)
        {
            Name = name;
            NameInstance = nameInstance;
            WorkManager = workManager;
            Watcher = watcher;

            _parameters = parameters ?? new NameValueCollection();
            _data = data;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public T GetData<T>(T defaultValue = default(T))
        {
            return (T)(_data ?? defaultValue);
        }

        /// <summary>
        /// Gets the specified parameter by key.
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
