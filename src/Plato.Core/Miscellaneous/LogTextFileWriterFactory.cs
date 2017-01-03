// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Core.Interfaces;

namespace Plato.Core.Miscellaneous
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Core.Interfaces.ILogTextFileWriterFactory" />
    public class LogTextFileWriterFactory : ILogTextFileWriterFactory
    {
        /// <summary>
        /// Creates the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="recycleNumber">The recycle number.</param>
        /// <param name="forceDirectoryCreation">if set to <c>true</c> [force directory creation].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ILogTextFileWriter Create(string fileName, int recycleNumber, bool forceDirectoryCreation)
        {
            return new LogTextFileWriter(fileName, recycleNumber, forceDirectoryCreation);
        }
    }
}
