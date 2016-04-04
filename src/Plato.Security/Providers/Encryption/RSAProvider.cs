// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Cryptography;
using Plato.Security.Interfaces;
using System;
using System.Collections.Specialized;
using System.Text;

namespace Plato.Security.Providers.Encryption
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Security.Interfaces.IEncryptionProvider" />
    public class RSAProvider : IEncryptionProvider
    {
        private string _xmlKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="RSAProvider"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentException">RSAProvider: missing parameter for xmlKey;xmlKey</exception>
        public RSAProvider(NameValueCollection parameters, params object[] args)
        {
            _xmlKey = parameters["xmlKey"];
            if (string.IsNullOrWhiteSpace(_xmlKey))
            {
                throw new ArgumentException("RSAProvider: missing parameter for xmlKey", "xmlKey");
            }
        }

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            using (var rsa = new RSAService(_xmlKey))
            {
                return rsa.EncryptArrayBlock(data);
            }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            using (var rsa = new RSAService(_xmlKey))
            {
                return rsa.DecryptArrayBlock(data);
            }
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string EncryptString(string data, Encoding encoding)
        {
            using (var rsa = new RSAService(_xmlKey))
            {
                return rsa.EncryptString(data, encoding);
            }
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public string DecryptString(string data, Encoding encoding)
        {
            using (var rsa = new RSAService(_xmlKey))
            {
                return rsa.DecryptString(data, encoding);
            }
        }
    }
}
