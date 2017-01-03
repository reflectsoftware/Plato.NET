// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Messaging.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum MessageExceptionCode
    {
        /// <summary>
        /// The lost connection
        /// </summary>
        LostConnection = 0,

        /// <summary>
        /// The exclusive lock
        /// </summary>
        ExclusiveLock = 1,

        /// <summary>
        /// The poison message
        /// </summary>
        PoisonMessage = 2,

        /// <summary>
        /// The no acceptable endpoints
        /// </summary>
        NoAcceptableEndpoints = 3,

        /// <summary>
        /// The unhandled error
        /// </summary>
        UnhandledError = 1000,
    }
}
