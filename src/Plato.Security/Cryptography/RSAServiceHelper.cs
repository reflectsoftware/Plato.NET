// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Security.Cryptography;
using System.Text;

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class RSAServiceHelper
    {
        /// <summary>
        /// Generates the XML key pair.
        /// </summary>
        /// <param name="bitStrength">The bit strength.</param>
        /// <returns></returns>
        public static string[] GenerateXmlKeyPair(int bitStrength = 1024)
        {
            using (var rsaProvider = new RSACryptoServiceProvider(bitStrength))
            {
                var keys = new string[2];
                
                keys[0] = rsaProvider.ToXmlString(true);
                keys[1] = rsaProvider.ToXmlString(false);

                rsaProvider.Clear();

                return keys;
            }
        }

        /// <summary>
        /// Encrypts the array block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static byte[] EncryptArrayBlock(byte[] data, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.EncryptArrayBlock(data);
            }
        }

        /// <summary>
        /// Decrypts the array block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static byte[] DecryptArrayBlock(byte[] data, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.DecryptArrayBlock(data);
            }
        }

        /// <summary>
        /// Encrypts the string block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static string EncryptStringBlock(string data, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.EncryptStringBlock(data);
            }
        }

        /// <summary>
        /// Decrypts the string block.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static string DecryptStringBlock(string data, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.DecryptStringBlock(data);
            }
        }

        /// <summary>
        /// Encrypts the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static byte[] EncryptBytes(byte[] bytes, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.EncryptBytes(bytes);
            }
        }

        /// <summary>
        /// Decrypts the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static byte[] DecryptBytes(byte[] bytes, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.DecryptBytes(bytes);
            }
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        public static string EncryptString(string data, string xmlKey, Encoding encoder)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.EncryptString(data, encoder);
            }
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <param name="encoder">The encoder.</param>
        /// <returns></returns>
        public static string DecryptString(string data, string xmlKey, Encoding encoder)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.DecryptString(data, encoder);
            }
        }

        /// <summary>
        /// Signs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static byte[] Sign(byte[] data, HashAlgorithm algo, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.Sign(data, algo);
            }
        }

        /// <summary>
        /// Signs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static byte[] Sign(byte[] data, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.Sign(data);
            }
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="originalData">The original data.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="algo">The algo.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static bool VerifySignature(byte[] originalData, byte[] signature, HashAlgorithm algo, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.VerifySignature(originalData, signature, algo);
            }
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="originalData">The original data.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="xmlKey">The XML key.</param>
        /// <returns></returns>
        public static bool VerifySignature(byte[] originalData, byte[] signature, string xmlKey)
        {
            using (var rsa = new RSAService(xmlKey))
            {
                return rsa.VerifySignature(originalData, signature);
            }
        }
    }
}
