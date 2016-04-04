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
    public class HybridEncryptionProvider : IEncryptionProvider
    {
        private CryptoNonce _nonce;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridEncryptionProvider"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentException">HybridEncryptionProvider: missing parameter for secret;secret</exception>
        public HybridEncryptionProvider(NameValueCollection parameters, params object[] args)
        {
            var secret = parameters["secret"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentException("HybridEncryptionProvider: missing parameter for secret", "secret");
            }

            var asymProvider = new AsymmetricEncryptionProvider(parameters);
            var eSecret = asymProvider.DecryptString(secret, Encoding.UTF8);
            var parts = eSecret.Split(new char[] { '|' });

            _nonce = new CryptoNonce()
            {
                Key = Convert.FromBase64String(parts[0]),
                IV = Convert.FromBase64String(parts[1])
            };
        }

        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] value)
        {
            return CryptoServices.AesEncrypt(value, _nonce);
        }

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] value)
        {
            return CryptoServices.AesDecrypt(value, _nonce);
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
