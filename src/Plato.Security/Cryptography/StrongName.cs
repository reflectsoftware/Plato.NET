// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using Plato.WinAPI;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Plato.Security.Cryptography
{
    /// <summary>   
    /// P/Invoke declarations for strong name APIs   
    /// </summary>
    public static class StrongName
    {        
        /// <summary>
        /// Generate a key.  Sizes have to be multiples of 1024
        /// </summary>
        /// <param name="keysize">size, in bits, of the key to generate</param>
        /// <returns>true if the operation succeeded, false otherwise</returns>
        public static byte[] GenerateKey(uint keysize)
        {
            var keyBlob = IntPtr.Zero;
            long generatedSize = 0;
            
            var createdKey = Mscoree.StrongNameKeyGenEx(
                null,
                Mscoree.StrongNameKeyGenFlags.None,
                (int)keysize,
                out keyBlob,
                out generatedSize);

            if (!createdKey || keyBlob == IntPtr.Zero)
            {
                var error = Marshal.GetExceptionForHR(Mscoree.StrongNameErrorInfo());
                throw error;
            }

            try
            {
                if (generatedSize <= 0 || generatedSize > int.MaxValue)
                {
                    throw new Exception("Error while generating key");
                }
                
                var key = new byte[generatedSize];
                Marshal.Copy(keyBlob, key, 0, (int)generatedSize);

                return key;
            }
            finally
            {
                if (keyBlob != IntPtr.Zero)
                {
                    Mscoree.StrongNameFreeBuffer(keyBlob);
                }
            }
        }
        /// <summary>
        /// Generate a key and write it to a file. Sizes have to be multiples of 1024
        /// </summary>
        /// <param name="keysize">size, in bits, of the key to generate</param>
        /// <param name="filename">name of the file to write to</param>
        /// <returns>true if the operation succeeded, false otherwise</returns>
        public static void GenerateKeyFile(uint keysize, string filename)
        {
            var key = GenerateKey(keysize);

            using (var snkStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (var snkWriter = new BinaryWriter(snkStream))
                {
                    snkWriter.Write(key);
                }
            }
        }
    }
}
