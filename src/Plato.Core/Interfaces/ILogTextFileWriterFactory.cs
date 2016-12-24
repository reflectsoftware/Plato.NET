// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogTextFileWriterFactory
    {
        /// <summary>
        /// Creates the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="recycleNumber">The recycle number.</param>
        /// <param name="forceDirectoryCreation">if set to <c>true</c> [force directory creation].</param>
        /// <returns></returns>
        ILogTextFileWriter Create(string fileName, int recycleNumber, bool forceDirectoryCreation);
    }
}
