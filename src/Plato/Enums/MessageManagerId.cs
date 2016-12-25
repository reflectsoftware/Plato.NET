// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Threading.Enums
{
    /// <summary>
    /// 
    /// </summary>
    internal enum MessageManagerId
    {
        /// <summary>
        /// The spawn worker
        /// </summary>
        SpawnWorker,

        /// <summary>
        /// The terminate worker
        /// </summary>
        TerminateWorker,

        /// <summary>
        /// The worker failed to respond
        /// </summary>
        WorkerFailedToRespond,

        /// <summary>
        /// The restart manager
        /// </summary>
        RestartManager
    }
}
