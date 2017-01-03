// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

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
    public sealed class SymAlgorithmServices
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="SymAlgorithmServices"/> class from being created.
        /// </summary>
        private SymAlgorithmServices()
        {
        }

        /// <summary>
        /// Validates the key iv.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="sa">The sa.</param>
        /// <exception cref="System.Security.Cryptography.CryptographicException">
        /// </exception>
        private static void ValidateKeyIV(byte[] key, byte[] IV, SymmetricAlgorithm sa )
        {
            if ( key.Length != sa.Key.Length )
            {
                var eMsg = string.Format("SymAlgorithmServices: -> Invalid key length for {0}. Keys must be {1} bytes in length.", sa.GetType().Name, sa.Key.Length );
                throw new CryptographicException( eMsg );
            }

            if ( IV.Length != sa.IV.Length )
            {
                var eMsg = string.Format("SymAlgorithmServices: -> Invalid IV length for {0}. IVs must be {1} bytes in length.", sa.GetType().Name, sa.IV.Length );
                throw new CryptographicException( eMsg );
            }
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <param name="cm">The cm.</param>
        /// <param name="sa">The sa.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] IV, byte[] data, CipherMode cm, SymmetricAlgorithm sa)
        {
            ValidateKeyIV(key, IV, sa);

            sa.Mode = cm;
            sa.Key = key;
            sa.IV = IV;

            using (var ms = new MemoryStream())
            {
                using (var ec = sa.CreateEncryptor())
                {
                    using (var cs = new CryptoStream(ms, ec, CryptoStreamMode.Write))
                    {
                        try
                        {
                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();
                            return ms.ToArray();
                        }
                        finally
                        {
                            cs.Clear();
                            cs.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <param name="cm">The cm.</param>
        /// <param name="sa">The sa.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] IV, byte[] data, CipherMode cm, SymmetricAlgorithm sa )
        {
            ValidateKeyIV( key, IV, sa );

            sa.Mode = cm;
            sa.Key  = key;
            sa.IV   = IV;

            using (var ms = new MemoryStream(data))
            {
                using (var dc = sa.CreateDecryptor())
                {
                    using (var cs = new CryptoStream(ms, dc, CryptoStreamMode.Read))
                    {
                        try
                        {
                            var rValue = new byte[data.Length];
                            cs.Read(rValue, 0, rValue.Length);
                            return rValue;
                        }
                        finally
                        {
                            cs.Clear();
                            cs.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <param name="cm">The cm.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] IV, byte[] data, CipherMode cm, string symName )
        {
            using (var sa = CryptoServices.CreateSymmetricAlgorithm(symName))
            {
                return Encrypt(key, IV, data, cm, sa);
            }
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] IV, byte[] data, string symName )
        {
            return Encrypt( key, IV, data, CipherMode.CBC, symName );
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] IV, byte[] data )
        {
            return Encrypt( key, IV, data, "RC2");
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="cm">The cm.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] data, CipherMode cm, string symName, string hashName )
        {
            var bSize = BitConverter.GetBytes(data.Length);
            var dataToEncrypt = new byte[data.Length + bSize.Length];
            Array.Copy(bSize, 0, dataToEncrypt, 0, bSize.Length);
            Array.Copy(data, 0, dataToEncrypt, bSize.Length, data.Length);

            using (var sa = CryptoServices.CreateSymmetricAlgorithm(symName))
            {
                try
                {
                    using (var ha = CryptoServices.CreateHashAlgorithm(hashName))
                    {
                        try
                        {
                            var IV = sa.IV;
                            var rKey = CryptoServices.InsureVector(ha.ComputeHash(key), sa.Key.Length, ha);
                            var rIV = CryptoServices.InsureVector(ha.ComputeHash(IV), sa.IV.Length, ha);
                            var eData = Encrypt(rKey, rIV, dataToEncrypt, cm, sa);
                            var rData = new byte[IV.Length + eData.Length];

                            Array.Copy(IV, 0, rData, 0, IV.Length);
                            Array.Copy(eData, 0, rData, IV.Length, eData.Length);

                            return rData;
                        }
                        finally
                        {                            
                            ha.Clear();
                        }
                    }
                }
                finally
                {
                    sa.Clear();
                }
            }
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] data, string symName, string hashName )
        {
            return Encrypt( key, data, CipherMode.CBC, symName, hashName );
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] data, string symName )
        {
            return Encrypt( key, data, symName, "MD5" );
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] data )
        {
            return Encrypt( key, data, "RC2", "MD5" );
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string Encrypt( string key, string data, string symName, string hashName )
        {
            var rValue = Encrypt( Encoding.ASCII.GetBytes( key ), Encoding.ASCII.GetBytes( data ), symName, hashName );
            return Convert.ToBase64String( rValue );
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static string Encrypt( string key, string data, string symName )
        {
            return Encrypt( key, data, symName, "MD5" );
        }

        /// <summary>
        /// Encrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Encrypt( string key, string data )
        {
            return Encrypt( key, data, "RC2", "MD5" );
        }

        /// <summary>
        /// Encrypts the unicode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string EncryptUnicode( string key, string data, string symName, string hashName )
        {
            var rValue = Encrypt( Encoding.ASCII.GetBytes( key ), Encoding.Unicode.GetBytes( data ), symName, hashName );
            return Convert.ToBase64String( rValue );
        }

        /// <summary>
        /// Encrypts the unicode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static string EncryptUnicode( string key, string data, string symName )
        {
            return EncryptUnicode( key, data, symName, "MD5" );
        }

        /// <summary>
        /// Encrypts the unicode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string EncryptUnicode( string key, string data )
        {
            return EncryptUnicode( key, data, "RC2", "MD5" );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <param name="cm">The cm.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] IV, byte[] data, CipherMode cm, string symName )
        {
            using (var sa = CryptoServices.CreateSymmetricAlgorithm(symName))
            {
                return Decrypt(key, IV, data, cm, sa);
            }
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] IV, byte[] data, string symName )
        {
            return Decrypt( key, IV, data, CipherMode.CBC, symName );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="IV">The iv.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] IV, byte[] data )
        {
            return Decrypt( key, IV, data, "RC2");
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="cm">The cm.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] data, CipherMode cm, string symName, string hashName )
        {
            using (var sa = CryptoServices.CreateSymmetricAlgorithm(symName))
            {
                try
                {
                    using (var ha = CryptoServices.CreateHashAlgorithm(hashName))
                    {
                        try
                        {
                            var IV = new byte[sa.IV.Length];
                            var eData = new byte[data.Length - IV.Length];

                            Array.Copy(data, 0, IV, 0, IV.Length);
                            Array.Copy(data, IV.Length, eData, 0, eData.Length);

                            var rKey = CryptoServices.InsureVector(ha.ComputeHash(key), sa.Key.Length, ha);
                            var rIV = CryptoServices.InsureVector(ha.ComputeHash(IV), sa.IV.Length, ha);

                            var tmpDecryptData = Decrypt(rKey, rIV, eData, cm, sa);
                            var rData = new byte[BitConverter.ToInt32(tmpDecryptData, 0)];
                            Array.Copy(tmpDecryptData, Marshal.SizeOf(rData.Length), rData, 0, rData.Length);

                            return rData;
                        }
                        finally
                        {                            
                            ha.Clear();
                        }
                    }
                }
                finally
                {
                    sa.Clear();
                }
            }
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] data, string symName, string hashName )
        {
            return Decrypt( key, data, CipherMode.CBC, symName, hashName );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] data, string symName )
        {
            return Decrypt( key, data, symName, "MD5" );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] data )
        {
            return Decrypt( key, data, "RC2", "MD5" );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string Decrypt( string key, string data, string symName, string hashName )
        {
            var rValue = Decrypt( Encoding.ASCII.GetBytes( key ), Convert.FromBase64String( data ), symName, hashName );
            return Encoding.ASCII.GetString( rValue );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static string Decrypt( string key, string data, string symName )
        {
            return Decrypt( key, data, symName, "MD5" );
        }

        /// <summary>
        /// Decrypts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Decrypt( string key, string data )
        {
            return Decrypt( key, data, "RC2", "MD5" );
        }

        /// <summary>
        /// Decrypts the unicode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <param name="hashName">Name of the hash.</param>
        /// <returns></returns>
        public static string DecryptUnicode( string key, string data, string symName, string hashName )
        {
            var rValue = Decrypt( Encoding.ASCII.GetBytes( key ), Convert.FromBase64String( data ), symName, hashName );
            return Encoding.Unicode.GetString( rValue );
        }

        /// <summary>
        /// Decrypts the unicode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="symName">Name of the sym.</param>
        /// <returns></returns>
        public static string DecryptUnicode( string key, string data, string symName )
        {
            return DecryptUnicode( key, data, symName, "MD5" );
        }

        /// <summary>
        /// Decrypts the unicode.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string DecryptUnicode( string key, string data )
        {
            return DecryptUnicode( key, data, "RC2", "MD5" );
        }
    }
}
