// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class DPAPI
    {
        /// <summary>
        /// Encrypts the specified plain data.
        /// </summary>
        /// <param name="plainData">The plain data.</param>
        /// <param name="entropyBytes">The entropy bytes.</param> 
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] plainData, byte[] entropyBytes, DataProtectionScope scope)
        {
            return ProtectedData.Protect(plainData, entropyBytes, scope);
        }

        /// <summary>
        /// Encrypts the specified plain data.
        /// </summary>
        /// <param name="plainData">The plain data.</param>
        /// <param name="entropyBytes">The entropy bytes.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] plainData, byte[] entropyBytes)
        {
            return Encrypt(plainData, entropyBytes, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Encrypts the specified plain data.
        /// </summary>
        /// <param name="plainData">The plain data.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] plainData, DataProtectionScope scope)
        {
            return Encrypt(plainData, null, scope);
        }

        /// <summary>
        /// Encrypts the specified plain data.
        /// </summary>
        /// <param name="plainData">The plain data.</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            return Encrypt(plainData, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts the specified encrypted data.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="entropyBytes">The entropy bytes.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encryptedData, byte[] entropyBytes, DataProtectionScope scope)
        {
            return ProtectedData.Unprotect(encryptedData, entropyBytes, scope);
        }

        /// <summary>
        /// Decrypts the specified encrypted data.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="entropyBytes">The entropy bytes.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encryptedData, byte[] entropyBytes)
        {
            return Decrypt(encryptedData, entropyBytes, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts the specified encrypted data.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encryptedData, DataProtectionScope scope)
        {
            return Decrypt(encryptedData, null, scope);
        }

        /// <summary>
        /// Decrypts the specified encrypted data.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] encryptedData)
        {
            return Decrypt(encryptedData, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="entropy">The entropy.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string entropy, DataProtectionScope scope)
        {
            return Convert.ToBase64String( Encrypt(Encoding.UTF8.GetBytes(plainText ?? string.Empty), Encoding.UTF8.GetBytes(entropy ?? string.Empty), scope));
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string entropy)
        {
            return Encrypt(plainText, entropy, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, DataProtectionScope scope)
        {
            return Encrypt(plainText, null, scope);
        }

        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, null, DataProtectionScope.CurrentUser);
        }

        public static string Decrypt(string encryptedText, string entropy, DataProtectionScope scope)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptedText), Encoding.UTF8.GetBytes(entropy ?? string.Empty), scope));
        }

        /// <summary>
        /// Decrypts the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText, string entropy)
        {
            return Decrypt(encryptedText, entropy, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText, DataProtectionScope scope)
        {
            return Decrypt(encryptedText, null, scope);
        }

        /// <summary>
        /// Decrypts the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText)
        {
            return Decrypt(encryptedText, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="bData">The b data.</param>
        /// <param name="scope">The scope.</param>
        public static void WriteFile(string fileName, byte[] bData, DataProtectionScope scope)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var bEncData = Encrypt(bData, scope);
                fs.Write(bEncData, 0, bEncData.Length);
            }
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="iostream">The iostream.</param>
        /// <param name="scope">The scope.</param>
        public static void WriteFile(string fileName, Stream iostream, DataProtectionScope scope)
        {
            var bData = new byte[iostream.Length];
            iostream.Position = 0;
            iostream.Read(bData, 0, bData.Length);

            WriteFile(fileName, bData, scope);
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="bData">The b data.</param>
        public static void WriteFile(string fileName, byte[] bData)
        {
            WriteFile(fileName, bData, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="iostream">The iostream.</param>
        public static void WriteFile(string fileName, Stream iostream)
        {
            WriteFile(fileName, iostream, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="data">The data.</param>
        /// <param name="scope">The scope.</param>
        public static void WriteFile(string fileName, string data, DataProtectionScope scope)
        {
            WriteFile(fileName, Encoding.UTF8.GetBytes(data), scope);
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="data">The data.</param>
        public static void WriteFile(string fileName, string data)
        {
            WriteFile(fileName, Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Reads the file byte array.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static byte[] ReadFileByteArray(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var bEncData = new byte[fs.Length];
                fs.Read(bEncData, 0, bEncData.Length);

                return Decrypt(bEncData);
            }
        }

        /// <summary>
        /// Reads the file stream.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Stream ReadFileStream(string fileName)
        {
            return new MemoryStream(ReadFileByteArray(fileName)) { Position = 0 };
        }

        /// <summary>
        /// Reads the file string.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string ReadFileString(string fileName)
        {
            return Encoding.UTF8.GetString(ReadFileByteArray(fileName));
        }

        public static SecureString CreateSecureString(string data)
        {
            var sString = new SecureString();
            foreach (char c in data.ToCharArray())
            {
                sString.AppendChar(c);
            }

            sString.MakeReadOnly();
            return sString;
        }

        /// <summary>
        /// Gets the secure string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetSecureString(SecureString data)
        {
            var ptr = Marshal.SecureStringToBSTR(data);
            try
            {
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ptr);
                }
            }
        }
    }
}
