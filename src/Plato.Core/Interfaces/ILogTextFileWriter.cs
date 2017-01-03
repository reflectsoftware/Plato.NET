// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;

namespace Plato.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ILogTextFileWriter : IDisposable
    {
        /// <summary>
        /// Gets the log file path.
        /// </summary>
        /// <value>
        /// The log file path.
        /// </value>
        string LogFilePath { get; }
       
        /// <summary>
        /// Gets a value indicating whether [create directory].
        /// </summary>
        /// <value>
        /// <c>true</c> if [create directory]; otherwise, <c>false</c>.
        /// </value>
        bool CreateDirectory { get; }
        
        /// <summary>
        /// Gets the recycle number.
        /// </summary>
        /// <value>
        /// The recycle number.
        /// </value>
        int RecycleNumber { get; }
        
        /// <summary>
        /// Gets a value indicating whether this <see cref="LogTextFileWriter"/> is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        bool Disposed { get; }

        /// <summary>
        /// Writes the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        void Write(string msg);

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="msg">The MSG.</param>        
        void WriteLine(string msg); 
    }
}
