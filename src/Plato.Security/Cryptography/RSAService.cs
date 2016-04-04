// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.Serializers;
using Plato.Serializers.Interfaces;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RSAService : IDisposable
    {
        private class NonceContainer : IFastBinarySerializable
        {
            /// <summary>
            /// Gets or sets the nonce.
            /// </summary>
            /// <value>
            /// The nonce.
            /// </value>
            public CryptoNonce Nonce { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="NonceContainer"/> class.
            /// </summary>
            /// <param name="nonce">The nonce.</param>
            public NonceContainer(CryptoNonce nonce)
            {
                Nonce = nonce;
            }

            /// <summary>
            /// Writes the data.
            /// </summary>
            /// <param name="writer">The writer.</param>
            /// <param name="additionalInfo">The additional information.</param>
            public void WriteData(FastBinaryWriter writer, object additionalInfo)
            {
                writer.WriteByteArray(Nonce.Key);
                writer.WriteByteArray(Nonce.IV);
            }

            /// <summary>
            /// Reads the data.
            /// </summary>
            /// <param name="reader">The reader.</param>
            /// <param name="additionalInfo">The additional information.</param>
            public void ReadData(FastBinaryReader reader, object additionalInfo)
            {
                Nonce = new CryptoNonce()
                {
                    Key = reader.ReadByteArray(),
                    IV = reader.ReadByteArray()
                };
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RSAService"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public RSACryptoServiceProvider Provider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RSAService"/> class.
        /// </summary>
        /// <param name="xmlKey">The XML key.</param>
        public RSAService(string xmlKey)
        {
            Disposed = false;
            Provider = new RSACryptoServiceProvider();
            Provider.FromXmlString(xmlKey);
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

                    if (Provider != null)
                    {
                        Provider.Clear();
                        Provider.Dispose();
                        Provider = null;
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the array block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] EncryptArrayBlock(byte[] data)
        {
            var nonceContainer = new NonceContainer(CryptoServices.CreateNonce(128));

            byte[] bEncryptedNonce;
            using (var ff = new FastBinaryFormatter())
            {
                bEncryptedNonce = EncryptBytes(ff.Serialize(nonceContainer));
            }
            
            var bEncryptedData = CryptoServices.AesEncrypt(data, nonceContainer.Nonce);
                        
            using (var ms = new MemoryStream())
            {
                var bBlobSize = BitConverter.GetBytes(bEncryptedNonce.Length);
                ms.Write(bBlobSize, 0, bBlobSize.Length);
                ms.Write(bEncryptedNonce, 0, bEncryptedNonce.Length);

                bBlobSize = BitConverter.GetBytes(bEncryptedData.Length);
                ms.Write(bBlobSize, 0, bBlobSize.Length);
                ms.Write(bEncryptedData, 0, bEncryptedData.Length);

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Decrypts the array block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] DecryptArrayBlock(byte[] data)
        {
            var bBloblSize = new byte[Marshal.SizeOf(typeof(int))];
            byte[] bEncryptedNonce;
            byte[] bEncryptedData;
            
            using (var ms = new MemoryStream(data))
            {
                ms.Read(bBloblSize, 0, bBloblSize.Length);
                bEncryptedNonce = new byte[BitConverter.ToInt32(bBloblSize, 0)];
                ms.Read(bEncryptedNonce, 0, bEncryptedNonce.Length);

                ms.Read(bBloblSize, 0, bBloblSize.Length);
                bEncryptedData = new byte[BitConverter.ToInt32(bBloblSize, 0)];
                ms.Read(bEncryptedData, 0, bEncryptedData.Length);
            }
            
            NonceContainer nonceContainer;
            using (var ff = new FastBinaryFormatter())
            {
                nonceContainer = ff.Deserialize<NonceContainer>(DecryptBytes(bEncryptedNonce));
            }

            return CryptoServices.AesDecrypt(bEncryptedData, nonceContainer.Nonce);
        }

        /// <summary>
        /// Encrypts the string block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public string EncryptStringBlock(string data)
        {
            return Convert.ToBase64String(EncryptArrayBlock(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// Decrypts the string block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public string DecryptStringBlock(string data)
        {
            return Encoding.UTF8.GetString(DecryptArrayBlock(Convert.FromBase64String(data)));
        }

        /// <summary>
        /// Encrypts the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public byte[] EncryptBytes(byte[] bytes)
        {
            return Provider.Encrypt(bytes, false);
        }

        /// <summary>
        /// Decrypts the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public byte[] DecryptBytes(byte[] bytes)
        {
            return Provider.Decrypt(bytes, false);
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        public string EncryptString(string data, Encoding encoder)
        {
            return Convert.ToBase64String(EncryptBytes(encoder.GetBytes(data)));
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        public string DecryptString(string data, Encoding encoder)
        {
            return encoder.GetString(DecryptBytes(Convert.FromBase64String(data)));
        }

        /// <summary>
        /// Signs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="algo">The algo.</param>
        /// <returns></returns>
        public byte[] Sign(byte[] data, HashAlgorithm algo)
        {
            return Provider.SignData(data, algo);
        }

        /// <summary>
        /// Signs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public byte[] Sign(byte[] data)
        {
            return Sign(data, SHA1.Create());
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="originalData">The original data.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="algo">The algo.</param>
        /// <returns></returns>
        public bool VerifySignature(byte[] originalData, byte[] signature, HashAlgorithm algo)
        {
            return Provider.VerifyData(originalData, algo, signature);
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="originalData">The original data.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        public bool VerifySignature(byte[] originalData, byte[] signature)
        {
            return VerifySignature(originalData, signature, SHA1.Create());
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Provider.Clear();
        }
    }
}
