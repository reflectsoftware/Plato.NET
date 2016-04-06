// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// This is a Asymmetric class. V2.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class Asymmetric : IDisposable
    {
        protected X509Certificate2 _cert;
        protected RSACryptoServiceProvider _cspEnc;
        protected RSACryptoServiceProvider _cspDec;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Asymmetric"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="Asymmetric"/> class from being created.
        /// </summary>
        private Asymmetric()
        {
            Disposed = false;
        }

        /// <summary>
        /// Gets the instance from certificate.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <returns></returns>
        public static Asymmetric GetInstanceFromCertificate(byte[] rawData)
        {
            var instance = new Asymmetric();
            var cert = new X509Certificate2(rawData);
            instance.SetCert(cert, GetCertThumbprint(cert));

            return instance;
        }

        /// <summary>
        /// Gets the instance from certificate.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        public static Asymmetric GetInstanceFromCertificate(X509Certificate2 cert, string certThumbprint)
        {
            var instance = new Asymmetric();
            instance.SetCert(cert, certThumbprint);

            return instance;
        }

        /// <summary>
        /// Gets the instance from PKC S12.
        /// </summary>
        /// <param name="pkcs12FilePath">The PKCS12 file path.</param>
        /// <param name="pkcs12Password">The PKCS12 password.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Empty pkcs#12 file path
        /// or
        /// Empty pkcs#12 file password
        /// </exception>
        public static Asymmetric GetInstanceFromPKCS12(string pkcs12FilePath, SecureString pkcs12Password, string certThumbprint)
        {
            if (null == pkcs12FilePath)
            {
                throw new ArgumentNullException(nameof(pkcs12FilePath));
            }

            if (null == pkcs12Password)
            {
                throw new ArgumentNullException(nameof(pkcs12Password));
            }

            var filepath = pkcs12FilePath.Trim();
            if (0 == filepath.Length)
            {
                throw new ArgumentException("Empty pkcs#12 file path");
            }

            if (0 == pkcs12Password.Length)
            {
                throw new ArgumentException("Empty pkcs#12 file password");
            }

            return GetInstanceFromCertificate(new X509Certificate2(filepath, pkcs12Password), certThumbprint);
        }
        
        /// <summary>
        /// Gets the instance from certificate path.
        /// </summary>
        /// <param name="certFilePath">The cert file path.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">Empty cert file path</exception>
        public static Asymmetric GetInstanceFromCertificatePath(string certFilePath, string certThumbprint)
        {
            if (null == certFilePath)
            {
                throw new ArgumentNullException(nameof(certFilePath));
            }

            var filepath = certFilePath.Trim();
            if (0 == filepath.Length)
            {
                throw new ArgumentException("Empty cert file path");
            }

            return GetInstanceFromCertificate(new X509Certificate2(filepath), certThumbprint);
        }

        /// <summary>
        /// Gets the instance from cert store.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLocation">The store location.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException">Could not find the specified certificate</exception>
        public static Asymmetric GetInstanceFromCertStore(string storeName, StoreLocation storeLocation, string certThumbprint)
        {
            if (null == storeName)
            {
                throw new ArgumentNullException(nameof(storeName));
            }

            if (null == certThumbprint)
            {
                throw new ArgumentNullException(nameof(certThumbprint));
            }

            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            try
            {
                var certs = store.Certificates;
                var foundCerts = certs.Find(X509FindType.FindByThumbprint, certThumbprint, false);

                if (foundCerts.Count != 1)
                {
                    throw new CryptographicException("Could not find the specified certificate");
                }

                return GetInstanceFromCertificate(foundCerts[0], certThumbprint);
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary>
        /// Gets the instance from cert store.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeLocation">The store location.</param>
        /// <param name="certThumbprint">The cert thumbprint.</param>
        /// <returns></returns>
        public static Asymmetric GetInstanceFromCertStore(StoreName storeName, StoreLocation storeLocation, string certThumbprint)
        {
            return GetInstanceFromCertStore(storeName.ToString(), storeLocation, certThumbprint);
        }

        /// <summary>
        /// Gets the instance from CSP BLOB.
        /// </summary>
        /// <param name="keyBlob">The key BLOB.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Bad key blob</exception>
        public static Asymmetric GetInstanceFromCSPBlob(byte[] keyBlob)
        {
            if (null == keyBlob || 0 == keyBlob.Length)
            {
                throw new ArgumentException("Bad key blob");
            }

            var instance = new Asymmetric();
            instance.SetCspBlob(keyBlob);

            return instance;
        }

        /// <summary>
        /// Gets the instance from PKC S12.
        /// </summary>
        /// <param name="pkcs12FilePath">The PKCS12 file path.</param>
        /// <param name="pkcs12Password">The PKCS12 password.</param>
        /// <returns></returns>
        public static Asymmetric GetInstanceFromPKCS12(string pkcs12FilePath, SecureString pkcs12Password)
        {
            return GetInstanceFromPKCS12(pkcs12FilePath, pkcs12Password, null);
        }

        /// <summary>
        /// Gets the instance from certificate path.
        /// </summary>
        /// <param name="certFilePath">The cert file path.</param>
        /// <returns></returns>
        public static Asymmetric GetInstanceFromCertificatePath(string certFilePath)
        {
            return GetInstanceFromCertificatePath(certFilePath, null);
        }

        /// <summary>
        /// Gets the cert thumbprint.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <returns></returns>
        public static string GetCertThumbprint(X509Certificate2 cert)
        {
            return cert.Thumbprint.Replace(" ", string.Empty).ToUpper();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    GC.SuppressFinalize(this);

                    if (_cspEnc != null)
                    {
                        _cspEnc.Clear();
                        _cspEnc.Dispose();
                        _cspEnc = null;
                    }

                    if (_cspDec != null)
                    {
                        _cspDec.Clear();
                        _cspDec.Dispose();
                        _cspDec = null;
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public byte[] EncryptBytes(byte[] data)
        {
            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return _cspEnc.Encrypt(data, true);
        }

        /// <summary>
        /// Decrypts the bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public byte[] DecryptBytes(byte[] data)
        {
            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return _cspDec.Decrypt(data, true);
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="instring">The instring.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public string EncryptString(string instring, Encoding encoding)
        {
            if (null == instring)
            {
                throw new ArgumentNullException(nameof(instring));
            }

            if (null == encoding)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var plain = encoding.GetBytes(instring);
            var cipher = EncryptBytes(plain);

            return Convert.ToBase64String(cipher);
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="instring">The instring.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Decryption is not supported since no private key is available</exception>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public string DecryptString(string instring, Encoding encoding)
        {
            if (null == _cspDec)
            {
                throw new InvalidOperationException("Decryption is not supported since no private key is available");
            }
            if (null == instring)
            {
                throw new ArgumentNullException(nameof(instring));
            }
            if (null == encoding)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            var cipher = Convert.FromBase64String(instring);
            var plain = DecryptBytes(cipher);

            var plainText = encoding.GetChars(plain, 0, plain.Length);

            return new string(plainText);
        }

        /// <summary>
        /// Sets the cert.
        /// </summary>
        /// <param name="cert">The cert.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <exception cref="System.Security.Cryptography.CryptographicException">Loaded certificate does not meet required certificate conditions.
        /// or
        /// The provided certificate is invalid. \r\n + _BuildCertValidationError(ch)</exception>
        private void SetCert(X509Certificate2 cert, string thumbprint)
        {
            if (null != thumbprint)
            {
                if (!cert.Thumbprint.Equals(thumbprint, StringComparison.OrdinalIgnoreCase))
                {
                    throw new CryptographicException("Loaded certificate does not meet required certificate conditions.");
                }
            }

            var ch = new X509Chain();
            ch.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

            var valid = ch.Build(cert);
            if (!valid)
            {
                throw new CryptographicException("The provided certificate is invalid. \r\n" + BuildCertValidationError(ch));
            }

            _cert = cert;
            _cspEnc = _cert.PublicKey.Key as RSACryptoServiceProvider;
            if (_cert.HasPrivateKey)
            {
                _cspDec = _cert.PrivateKey as RSACryptoServiceProvider;
            }
        }

        /// <summary>
        /// Sets the CSP BLOB.
        /// </summary>
        /// <param name="keyBlob">The key BLOB.</param>
        private void SetCspBlob(byte[] keyBlob)
        {
            _cspEnc = new RSACryptoServiceProvider();
            _cspEnc.ImportCspBlob(keyBlob);
        }

        /// <summary>
        /// Builds the cert validation error.
        /// </summary>
        /// <param name="chain">The chain.</param>
        /// <returns></returns>
        private static string BuildCertValidationError(X509Chain chain)
        {
            var sb = new StringBuilder();
            var statusCol = chain.ChainStatus;
            for (var x = 0; x < chain.ChainStatus.Length; x++)
            {
                var status = statusCol[x];
                sb.Append(status.Status.ToString()).Append("-->").Append(status.StatusInformation).AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Exports the public key to file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void ExportPublicKeyToFile(string filePath)
        {
            var blob = _cspEnc.ExportCspBlob(false);

            using (var fs = new FileStream(filePath, FileMode.CreateNew))
            {
                using (var sr = new StreamWriter(fs))
                {
                    for (var x = 0; x < blob.Length; x++)
                    {
                        if (x > 0 && (x % 16) == 0)
                        {
                            sr.WriteLine();
                        }

                        var s = string.Format("{0,2:X02}", blob[x]);
                        sr.Write((s + " "));
                    }
                }
            }
        }
    }
}
