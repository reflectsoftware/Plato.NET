// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Interfaces.IBaseThread" />
    public interface IBaseWorker : Plato.Interfaces.IBaseThread
    {
        /// <summary>
        /// Gets the work package.
        /// </summary>
        /// <value>
        /// The work package.
        /// </value>
        IWorkPackage WorkPackage { get; }

        /// <summary>
        /// Joins the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="waitmsec">The waitmsec.</param>
        void Join(IBaseWorker worker, int waitmsec);

        /// <summary>
        /// Joins the specified worker.
        /// </summary>
        /// <param name="worker">The worker.</param>
        void Join(IBaseWorker worker);
    }
}
