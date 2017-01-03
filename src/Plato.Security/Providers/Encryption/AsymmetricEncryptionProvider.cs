// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Security.Cryptography;
using Plato.Security.Interfaces;
using System;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Plato.Security.Providers.Encryption
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Plato.Security.Interfaces.IEncryptionProvider" />
    public class AsymmetricEncryptionProvider : IEncryptionProvider
    {
        private string _thumbprintParam;
        private string _storeNameParam;
        private StoreLocation _storeLocationParam;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AsymmetricEncryptionProvider"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.ArgumentException">
        /// AsymmetricEncryptionProvider: missing parameter for storeName;storeName
        /// or
        /// AsymmetricEncryptionProvider: missing parameter for storeLocation;storeLocation
        /// or
        /// AsymmetricEncryptionProvider: missing parameter for thumbprint;thumbprint
        /// or
        /// storeLocation
        /// </exception>
        public AsymmetricEncryptionProvider(NameValueCollection parameters, params object[] args)
        {
            _storeNameParam = parameters["storeName"];
            if (string.IsNullOrWhiteSpace(_storeNameParam))
            {
                throw new ArgumentException("AsymmetricEncryptionProvider: missing parameter for storeName", "storeName");
            }

            var storeLocation = parameters["storeLocation"];
            if (string.IsNullOrWhiteSpace(storeLocation))
            {
                throw new ArgumentException("AsymmetricEncryptionProvider: missing parameter for storeLocation", "storeLocation");
            }

            _thumbprintParam = parameters["thumbprint"];
            if (string.IsNullOrWhiteSpace(_thumbprintParam))
            {
                throw new ArgumentException("AsymmetricEncryptionProvider: missing parameter for thumbprint", "thumbprint");
            }

            StoreLocation sloc;
            if (!Enum.TryParse(storeLocation, out sloc))
            {
                throw new ArgumentException(string.Format("AsymmetricEncryptionProvider: Invalid parameter for storeLocation: '{0}'", storeLocation), "storeLocation");
            }

            _storeLocationParam = sloc;
            _thumbprintParam = Certificate.FormatThumbprint(_thumbprintParam);
        }

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            using (var asym = Asymmetric.GetInstanceFromCertStore(_storeNameParam, _storeLocationParam, _thumbprintParam))
            {
                return asym.EncryptBytes(data);
            }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            using (var asym = Asymmetric.GetInstanceFromCertStore(_storeNameParam, _storeLocationParam, _thumbprintParam))
            {
                return asym.DecryptBytes(data);
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
            using (var asym = Asymmetric.GetInstanceFromCertStore(_storeNameParam, _storeLocationParam, _thumbprintParam))
            {
                return asym.EncryptString(data, encoding);
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
            using (var asym = Asymmetric.GetInstanceFromCertStore(_storeNameParam, _storeLocationParam, _thumbprintParam))
            {
                return asym.DecryptString(data, encoding);
            }
        }
    }
}
