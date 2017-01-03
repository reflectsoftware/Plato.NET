// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Interfaces;
using System;
using System.Text;
using System.Web.Security;

namespace Plato.Security.Providers.Encryption
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Security.Interfaces.IEncryptionProvider" />
    public class MachineKeyProvider : IEncryptionProvider
    {
        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] value)
        {
            return MachineKey.Protect(value);
        }

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] value)
        {
            return MachineKey.Unprotect(value);
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string EncryptString(string value, Encoding encoding)
        {
            return Convert.ToBase64String(Encrypt(encoding.GetBytes(value)));
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string DecryptString(string value, Encoding encoding)
        {
            return encoding.GetString(Decrypt(Convert.FromBase64String(value)));
        }
    }
}
