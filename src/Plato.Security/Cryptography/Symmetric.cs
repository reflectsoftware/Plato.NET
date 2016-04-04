// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class Symmetric : IDisposable
    {
        protected const int BLOCK_MULTIPLIER = 64;
        protected const int KEY_SIZE = 32;

        protected Rijndael _csp;
        protected byte[] _keyBytes;
        protected byte[] _ivBytes;
        protected int _blockSize;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Symmetric"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Symmetric"/> class.
        /// </summary>
        /// <param name="passPhrase">The pass phrase.</param>
        /// <param name="iv">The iv.</param>
        /// <exception cref="System.ArgumentException">
        /// Bad pass phrase
        /// or
        /// Bad initialization vector
        /// </exception>
        public Symmetric(string passPhrase, string iv)
        {
            Disposed = false;

            if (null == passPhrase || 8 > passPhrase.Length)
            {
                throw new ArgumentException("Bad pass phrase");
            }

            if (null == iv || 8 > iv.Length)
            {
                throw new ArgumentException("Bad initialization vector");
            }

            _csp = new RijndaelManaged();
            _csp.Mode = CipherMode.CBC;
            _csp.Padding = PaddingMode.PKCS7;
            _blockSize = (_csp.BlockSize / 8) * BLOCK_MULTIPLIER;

            Encoding encoder = new UTF8Encoding(false, true);
            
            _ivBytes = CryptoServices.ComputeHash( encoder.GetBytes(iv), "SHA256");
            Array.Resize(ref _ivBytes, 16);
            
            var pdb = new PasswordDeriveBytes(passPhrase, _ivBytes, "SHA256", 100);
            _keyBytes = pdb.GetBytes(KEY_SIZE);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Symmetric"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <exception cref="System.ArgumentException">
        /// Bad key bytes
        /// or
        /// Bad initialization vector bytes
        /// </exception>
        public Symmetric(byte[] key, byte[] iv)
        {
            Disposed = false;

            if (null == key || 16 > key.Length)
            {
                throw new ArgumentException("Bad key bytes");
            }

            if (null == iv || 16 != iv.Length)
            {
                throw new ArgumentException("Bad initialization vector bytes");
            }

            _csp = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };
            _ivBytes = (byte[])iv.Clone();
            _keyBytes = (byte[])key.Clone();
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

                    if (_csp != null)
                    {
                        _csp.Clear();
                        _csp.Dispose();
                        _csp = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the stream encryptor.
        /// </summary>
        /// <param name="instream">The instream.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Bad input stream for encryption</exception>
        public CryptoStream GetStreamEncryptor(Stream instream)
        {
            if (null == instream)
            {
                throw new ArgumentException("Bad input stream for encryption");
            }

            var _encryptor = _csp.CreateEncryptor(_keyBytes, _ivBytes);
            return new CryptoStream(instream, _encryptor, CryptoStreamMode.Write);
        }

        /// <summary>
        /// Gets the file encryptor.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Bad file name for file encryption
        /// or
        /// File already exists
        /// </exception>
        public CryptoStream GetFileEncryptor(string filePath, bool overwrite)
        {
            if (null == filePath || 0 == filePath.Length)
            {
                throw new ArgumentException("Bad file name for file encryption");
            }

            if (File.Exists(filePath) && !overwrite)
            {
                throw new ArgumentException("File already exists");
            }

            return GetStreamEncryptor( new FileStream(filePath, FileMode.Create) );
        }

        /// <summary>
        /// Gets the file encryptor.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public CryptoStream GetFileEncryptor(string filePath)
        {
            return GetFileEncryptor(filePath, true);
        }

        /// <summary>
        /// Gets the stream decryptor.
        /// </summary>
        /// <param name="instream">The instream.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Bad input stream for decryption</exception>
        public CryptoStream GetStreamDecryptor(Stream instream)
        {
            if (null == instream)
            {
                throw new ArgumentException("Bad input stream for decryption");
            }

            var _decryptor = _csp.CreateDecryptor(_keyBytes, _ivBytes);
            return new CryptoStream(instream, _decryptor, CryptoStreamMode.Read);
        }

        public CryptoStream GetFileDecryptor(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File does not exist");
            }

            return GetStreamDecryptor( new FileStream(filePath, FileMode.Open, FileAccess.Read) );
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="instream">The instream.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <exception cref="System.ArgumentException">Bad input stream for file encryption</exception>
        public void EncryptFile(Stream instream, string filePath, bool overwrite)
        {
            if (null == instream)
            {
                throw new ArgumentException("Bad input stream for file encryption");
            }

            var cs = GetFileEncryptor(filePath, overwrite);
            try
            {
                instream.Seek(0, SeekOrigin.Begin);

                var buf = new byte[_blockSize];
                var read = 0;

                do
                {
                    read = instream.Read(buf, 0, _blockSize);
                    cs.Write(buf, 0, read);
                }
                while (read > 0);
            }
            finally
            {
                cs.Clear();
                cs.Close();
            }
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="instream">The instream.</param>
        /// <param name="filePath">The file path.</param>
        public void EncryptFile(Stream instream, string filePath)
        {
            EncryptFile(instream, filePath, true);
        }

        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Bad file name for file encryption</exception>
        public MemoryStream DecryptFile(string filePath)
        {
            if (null == filePath || 0 == filePath.Length)
            {
                throw new ArgumentException("Bad file name for file encryption");
            }

            var ms = new MemoryStream();
            using (var cs = GetFileDecryptor(filePath))
            {
                try
                {
                    var buf = new byte[_blockSize];
                    var read = 0;

                    do
                    {
                        read = cs.Read(buf, 0, _blockSize);
                        ms.Write(buf, 0, read);
                    }
                    while (read > 0);

                    ms.Seek(0, SeekOrigin.Begin);
                    return ms;
                }
                finally
                {
                    cs.Clear();
                    cs.Close();
                }
            }
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="instring">The instring.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Bad input string
        /// or
        /// Bad encryption encoding
        /// </exception>
        public string EncryptString(string instring, Encoding encoding)
        {
            if (null == instring)
            {
                throw new ArgumentException("Bad input string");
            }

            if (null == encoding)
            {
                throw new ArgumentException("Bad encryption encoding");
            }

            var plain = encoding.GetBytes(instring);
            var cipher = EncryptBytes(plain);
            var cipherString = Convert.ToBase64String(cipher);

            return cipherString;
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="instring">The instring.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Bad input string
        /// or
        /// Bad decryption encoding
        /// </exception>
        public string DecryptString(string instring, Encoding encoding)
        {
            if (null == instring)
            {
                throw new ArgumentException("Bad input string");
            }

            if (null == encoding)
            {
                throw new ArgumentException("Bad decryption encoding");
            }

            var cipher = Convert.FromBase64String(instring);
            var plain = DecryptBytes(cipher);
            var plainText = encoding.GetChars(plain, 0, plain.Length);
            var plainString = new string(plainText);

            return plainString;
        }

        /// <summary>
        /// Encrypts the bytes.
        /// </summary>
        /// <param name="inbytes">The inbytes.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Bad input bytes</exception>
        public byte[] EncryptBytes(byte[] inbytes)
        {
            if (null == inbytes || 0 == inbytes.Length)
            {
                throw new ArgumentException("Bad input bytes");
            }

            using (var ms = new MemoryStream())
            {
                using (var cs = GetStreamEncryptor(ms))
                {
                    try
                    {
                        cs.Write(inbytes, 0, inbytes.Length);
                        cs.FlushFinalBlock();
                        ms.Seek(0, SeekOrigin.Begin);

                        var cipher = ms.ToArray();
                        return cipher;
                    }
                    finally
                    {
                        cs.Clear();
                        cs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts the bytes.
        /// </summary>
        /// <param name="inbytes">The inbytes.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Bad input bytes</exception>
        public byte[] DecryptBytes(byte[] inbytes)
        {
            if (null == inbytes || 0 == inbytes.Length)
            {
                throw new ArgumentException("Bad input bytes");
            }

            using (var ms = new MemoryStream())
            {
                using (var cs = GetStreamDecryptor(ms))
                {
                    try
                    {
                        ms.Write(inbytes, 0, inbytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);

                        var plain = new byte[inbytes.Length];
                        var read = cs.Read(plain, 0, plain.Length);
                        Array.Resize(ref plain, read);

                        return plain;
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
}
