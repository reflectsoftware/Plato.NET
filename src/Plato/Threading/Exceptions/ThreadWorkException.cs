// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Threading.Exceptions
{
    /// <summary>
    /// Summary description for ThreadWorkException.
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    [Serializable]
    public class ThreadWorkException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWorkException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public ThreadWorkException(string msg) : base( msg )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWorkException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="innerException">The inner exception.</param>
        public ThreadWorkException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}
