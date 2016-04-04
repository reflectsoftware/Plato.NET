// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Text;

namespace Plato.Security.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        byte[] Encrypt(byte[] data);

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        byte[] Decrypt(byte[] data);

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        string EncryptString(string data, Encoding encoding);

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        string DecryptString(string data, Encoding encoding);
    }
}
