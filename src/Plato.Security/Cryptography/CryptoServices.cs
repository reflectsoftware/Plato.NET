// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

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
    public static class CryptoServices
    {
        /// <summary>
        /// Creates the symmetric algorithm.
        /// </summary>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static SymmetricAlgorithm CreateSymmetricAlgorithm(string symName)
        {
            var sa = SymmetricAlgorithm.Create(symName.Trim());
            if (sa == null)
            {
                var eMsg = string.Format("SymAlgorithmServices: -> SymmetricAlgorithm '{0}' is not supported.", symName);
                throw new CryptographicException(eMsg);
            }

            return sa;
        }

        /// <summary>
        /// Creates the hash algorithm.
        /// </summary>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static HashAlgorithm CreateHashAlgorithm(string hashName)
        {
            var ha = HashAlgorithm.Create(hashName.Trim());
            if (ha == null)
            {
                var eMsg = string.Format("SymAlgorithmServices: -> HashAlgorithm '{0}' is not supported.", hashName);
                throw new CryptographicException(eMsg);
            }

            return ha;
        }

        /// <summary>
        /// Generates the aes key.
        /// </summary>
        /// <param name="keySize">Size of the key.</param>
        /// <returns></returns>
        public static byte[] GenerateAesKey(int keySize)
        {
            using (var myRijndael = new RijndaelManaged())
            {
                try
                {
                    myRijndael.KeySize = keySize;
                    myRijndael.GenerateKey();
                    return myRijndael.Key;
                }
                finally
                {
                    myRijndael.Clear();
                }
            }
        }

        /// <summary>
        /// Generates the aes key.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateAesKey()
        {
            return GenerateAesKey(256);
        }

        /// <summary>
        /// Generates the aes key to base64.
        /// </summary>
        /// <returns></returns>
        public static string GenerateAesKeyToBase64()
        {
            return Convert.ToBase64String(GenerateAesKey());
        }

        /// <summary>
        /// Generates the aes key to base64.
        /// </summary>
        /// <param name="keySize">Size of the key.</param>
        /// <returns></returns>
        public static string GenerateAesKeyToBase64(int keySize)
        {
            return Convert.ToBase64String(GenerateAesKey(keySize));
        }

        public static byte[] GenerateAesIV()
        {
            using (var myRijndael = new RijndaelManaged())
            {
                try
                {
                    myRijndael.GenerateIV();
                    return myRijndael.IV;
                }
                finally
                {
                    myRijndael.Clear();
                }
            }
        }

        /// <summary>
        /// Generates the aes iv to base64.
        /// </summary>
        /// <returns></returns>
        public static string GenerateAesIVToBase64()
        {
            return Convert.ToBase64String(GenerateAesIV());
        }

        public static CryptoNonce CreateNonce(int keySize)
        {
            return new CryptoNonce() { Key = GenerateAesKey(keySize), IV = GenerateAesIV() };
        }

        /// <summary>
        /// Aeses the encrypt.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] src, byte[] key, byte[] IV)
        {
            using (var myRijndael = new RijndaelManaged())
            {
                try
                {
                    myRijndael.Mode = CipherMode.CBC;
                    myRijndael.Key = key;
                    myRijndael.IV = IV;
                    myRijndael.Padding = PaddingMode.PKCS7;

                    using (var encryptor = myRijndael.CreateEncryptor())
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(src, 0, src.Length);
                                csEncrypt.FlushFinalBlock();

                                return msEncrypt.ToArray();
                            }
                        }
                    }
                }
                finally
                {
                    myRijndael.Clear();
                }
            }
        }

        /// <summary>
        /// Aeses the encrypt.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] src, CryptoNonce nonce)
        {
            return AesEncrypt(src, nonce.Key, nonce.IV);
        }

        public static byte[] AesDecrypt(byte[] src, byte[] key, byte[] IV)
        {
            using (var myRijndael = new RijndaelManaged())
            {
                try
                {
                    myRijndael.Mode = CipherMode.CBC;
                    myRijndael.Key = key;
                    myRijndael.IV = IV;
                    myRijndael.Padding = PaddingMode.PKCS7;

                    using (var decryptor = myRijndael.CreateDecryptor())
                    {
                        using (var msDecrypt = new MemoryStream())
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                            {
                                csDecrypt.Write(src, 0, src.Length);
                                csDecrypt.FlushFinalBlock();

                                return msDecrypt.ToArray();
                            }
                        }
                    }
                }
                finally
                {
                    myRijndael.Clear();
                }
            }
        }

        /// <summary>
        /// Aeses the decrypt.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] src, CryptoNonce nonce)
        {
            return AesDecrypt(src, nonce.Key, nonce.IV);
        }

        /// <summary>
        /// Aeses the encrypt to64 base string.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static string AesEncryptTo64BaseString(byte[] src, byte[] key, byte[] IV)
        {
            return Convert.ToBase64String(AesEncrypt(src, key, IV));
        }

        /// <summary>
        /// Aeses the encrypt to64 base string.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static string AesEncryptTo64BaseString(byte[] src, CryptoNonce nonce)
        {
            return AesEncryptTo64BaseString(src, nonce.Key, nonce.IV);
        }

        /// <summary>
        /// Aeses the decrypt from64 base string.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static byte[] AesDecryptFrom64BaseString(string src, byte[] key, byte[] IV)
        {
            return AesDecrypt(Convert.FromBase64String(src), key, IV);
        }

        /// <summary>
        /// Aeses the decrypt from64 base string.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static byte[] AesDecryptFrom64BaseString(string src, CryptoNonce nonce)
        {
            return AesDecryptFrom64BaseString(src, nonce.Key, nonce.IV);
        }

        /// <summary>
        /// Aeses the encrypt serialize object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="obj">The object.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static byte[] AesEncryptSerializeObject(IObjectSerializer serializer, object obj, byte[] key, byte[] IV)
        {
            return AesEncrypt(serializer.Serialize(obj), key, IV);
        }

        /// <summary>
        /// Aeses the encrypt serialize object.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="obj">The object.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static byte[] AesEncryptSerializeObject(IObjectSerializer serializer, object obj, CryptoNonce nonce)
        {
            return AesEncryptSerializeObject(serializer, obj, nonce.Key, nonce.IV);
        }

        /// <summary>
        /// Aeses the decrypt deserialize object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">The serializer.</param>
        /// <param name="bObj">The b object.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static T AesDecryptDeserializeObject<T>(IObjectSerializer serializer, byte[] bObj, byte[] key, byte[] IV) where T : class
        {
            return serializer.Deserialize<T>(AesDecrypt(bObj, key, IV));
        }

        /// <summary>
        /// Aeses the decrypt deserialize object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">The serializer.</param>
        /// <param name="bObj">The b object.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static T AesDecryptDeserializeObject<T>(IObjectSerializer serializer, byte[] bObj, CryptoNonce nonce) where T : class
        {
            return AesDecryptDeserializeObject<T>(serializer, bObj, nonce.Key, nonce.IV);
        }

        /// <summary>
        /// Aeses the encrypt serialize to64 base string.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="obj">The object.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static string AesEncryptSerializeTo64BaseString(IObjectSerializer serializer, object obj, byte[] key, byte[] IV)
        {
            return Convert.ToBase64String(AesEncryptSerializeObject(serializer, obj, key, IV));
        }

        /// <summary>
        /// Aeses the encrypt serialize to64 base string.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="obj">The object.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static string AesEncryptSerializeTo64BaseString(IObjectSerializer serializer, object obj, CryptoNonce nonce)
        {
            return AesEncryptSerializeTo64BaseString(serializer, obj, nonce.Key, nonce.IV);
        }

        /// <summary>
        /// Aeses the decrypt deserialize64 base string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">The serializer.</param>
        /// <param name="sObj">The s object.</param>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <returns></returns>
        public static T AesDecryptDeserialize64BaseString<T>(IObjectSerializer serializer, string sObj, byte[] key, byte[] IV) where T : class
        {
            return AesDecryptDeserializeObject<T>(serializer, Convert.FromBase64String(sObj), key, IV);
        }

        /// <summary>
        /// Aeses the decrypt deserialize64 base string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">The serializer.</param>
        /// <param name="sObj">The s object.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static T AesDecryptDeserialize64BaseString<T>(IObjectSerializer serializer, string sObj, CryptoNonce nonce) where T : class
        {
            return AesDecryptDeserialize64BaseString<T>(serializer, sObj, nonce.Key, nonce.IV);
        }
        public static byte[] InsureVector(byte[] vector, int maxSize, HashAlgorithm ha)
        {
            if (vector.Length != maxSize)
            {
                var rKey = new byte[maxSize];
                var diff = maxSize - vector.Length;
                if (diff < 0)
                {
                    Array.Copy(vector, 0, rKey, 0, maxSize);
                }
                else
                {
                    if (diff > 0)
                    {
                        var padding = ha.ComputeHash(RNG(diff));
                        Array.Copy(vector, 0, rKey, 0, vector.Length);
                        Array.Copy(padding, 0, rKey, vector.Length, diff);
                    }
                }

                vector = rKey;
            }

            return vector;
        }

        /// <summary>
        /// Insures the vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] InsureVector(byte[] vector, int maxSize, string hashName)
        {
            return InsureVector(vector, maxSize, CreateHashAlgorithm(hashName));
        }

        /// <summary>
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] RNG(int size)
        {
            var rValue = new byte[size];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(rValue);
            }

            return rValue;
        }

        /// <summary>
        /// RNGs the base64 string.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string RNGBase64String(int size)
        {
            return Convert.ToBase64String(RNG(size));
        }

        /// <summary>
        /// Randoms the identifier to u int32.
        /// </summary>
        /// <returns></returns>
        public static uint RandomIdToUInt32()
        {
            return BitConverter.ToUInt32(RNG(Marshal.SizeOf(typeof(uint))), 0);
        }

        /// <summary>
        /// Randoms the identifier to u int64.
        /// </summary>
        /// <returns></returns>
        public static ulong RandomIdToUInt64()
        {
            return BitConverter.ToUInt64(RNG(Marshal.SizeOf(typeof(ulong))), 0);
        }

        public static byte[] ComputeHash(byte[] buffer, int offset, int count, string hashName)
        {
            using (var ha = CreateHashAlgorithm(hashName))
            {
                try
                {
                    return ha.ComputeHash(buffer, offset, count);
                }
                finally
                {
                    ha.Clear();
                }
            }
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] buffer, string hashName)
        {
            using (var ha = CreateHashAlgorithm(hashName))
            {
                try
                {
                    return ha.ComputeHash(buffer);
                }
                finally
                {
                    ha.Clear();
                }
            }
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] ComputeHash(Stream inputStream, string hashName)
        {
            using (var ha = CreateHashAlgorithm(hashName))
            {
                try
                {
                    return ha.ComputeHash(inputStream);
                }
                finally
                {
                    ha.Clear();
                }
            }
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            return ComputeHash(buffer, offset, count, "MD5");
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, "MD5");
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        public static byte[] ComputeHash(Stream inputStream)
        {
            return ComputeHash(inputStream, "MD5");
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(byte[] buffer, int offset, int count, string hashName)
        {
            return Convert.ToBase64String(ComputeHash(buffer, offset, count, hashName));
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(byte[] buffer, string hashName)
        {
            return Convert.ToBase64String(ComputeHash(buffer, hashName));
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(Stream inputStream, string hashName)
        {
            return Convert.ToBase64String(ComputeHash(inputStream, hashName));
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(byte[] buffer, int offset, int count)
        {
            return Convert.ToBase64String(ComputeHash(buffer, offset, count));
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(byte[] buffer)
        {
            return Convert.ToBase64String(ComputeHash(buffer));
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(Stream inputStream)
        {
            return Convert.ToBase64String(ComputeHash(inputStream));
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(string input, string hashName)
        {
            return ComputeHashBase64String(Encoding.ASCII.GetBytes(input), hashName);
        }

        /// <summary>
        /// Computes the hash base64 string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ComputeHashBase64String(string input)
        {
            return ComputeHashBase64String(Encoding.ASCII.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash unicode base64 string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string ComputeHashUnicodeBase64String(string input, string hashName)
        {
            return ComputeHashBase64String(Encoding.Unicode.GetBytes(input), hashName);
        }

        /// <summary>
        /// Computes the hash unicode get base64 string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ComputeHashUnicodeGetBase64String(string input)
        {
            return ComputeHashBase64String(Encoding.Unicode.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash get bytes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] ComputeHashGetBytes(string input, string hashName)
        {
            return ComputeHash(Encoding.ASCII.GetBytes(input), hashName);
        }

        /// <summary>
        /// Computes the hash get bytes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static byte[] ComputeHashGetBytes(string input)
        {
            return ComputeHash(Encoding.ASCII.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash unicode get bytes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] ComputeHashUnicodeGetBytes(string input, string hashName)
        {
            return ComputeHash(Encoding.Unicode.GetBytes(input), hashName);
        }

        /// <summary>
        /// Computes the hash unicode get bytes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static byte[] ComputeHashUnicodeGetBytes(string input)
        {
            return ComputeHash(Encoding.Unicode.GetBytes(input));
        }

        /// <summary>
        /// Generates the hmacsh a26 result.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static HMACResult GenerateHMACSHA26Result(string data, string privateKey, Encoding encoding)
        {
            using (var hmacsha256 = new HMACSHA256(encoding.GetBytes(privateKey)))
            {
                return new HMACResult()
                {
                    Data = data,
                    Signature = Convert.ToBase64String(hmacsha256.ComputeHash(encoding.GetBytes(data)))
                };
            }
        }
    }
}
