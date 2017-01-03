// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Threading.Exceptions;
using Plato.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Plato.Threading.WorkManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkManagerRegistry
    {
        private readonly Dictionary<string, WorkerRegisteryPackage> _workers;

        /// <summary>
        /// Gets or sets the dependency resolver.
        /// </summary>
        /// <value>
        /// The dependency resolver.
        /// </value>
        internal IWorkManagerDependencyResolver DependencyResolver { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkManagerRegistry"/> class.
        /// </summary>
        public WorkManagerRegistry()
        {
            _workers = new Dictionary<string, WorkerRegisteryPackage>();
        }

        /// <summary>
        /// Registers the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <param name="parameters">The parameters.</param>
        public void Register<T>(string name, bool active, NameValueCollection parameters = null)
        {
            _workers[name] = new WorkerRegisteryPackage(name, typeof(T), active, parameters);
        }

        /// <summary>
        /// Gets all active workers.
        /// </summary>
        /// <returns></returns>
        internal List<WorkerRegisteryPackage> GetAllActiveWorkers()
        {
            return _workers.Values.Where(x => x.Active).ToList();
        }

        /// <summary>
        /// Gets the worker.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal WorkerRegisteryPackage GetWorker(string name)
        {
            return _workers.Values.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="workPackage">The work package.</param>
        /// <returns></returns>
        /// <exception cref="WorkManagerException"></exception>
        /// <exception cref="System.NullReferenceException">Must supply a valid WorkManagerDependencyResolver.</exception>
        internal T Resolve<T>(string name, WorkPackage workPackage)
        {
            var regPackage = _workers.ContainsKey(name) ? _workers[name] : null;
            if (regPackage == null)
            {
                throw new WorkManagerException(string.Format("No worker registry was located for name: '{0}'.", name));
            }

            if (DependencyResolver == null)
            {
                throw new NullReferenceException("Must supply a valid WorkManagerDependencyResolver.");
            }

            return DependencyResolver.Resolve<T>(name, regPackage.Type, workPackage);
        }
    }
}
