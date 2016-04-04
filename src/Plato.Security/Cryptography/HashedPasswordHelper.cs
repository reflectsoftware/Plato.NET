// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public static class HashedPasswordHelper
    {
        /// <summary>
        /// Generates the salt hash.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string GenerateSaltHash(int size)
        {
            return CryptoServices.RNGBase64String(size);
        }

        /// <summary>
        /// Generates the password hash.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <returns></returns>
        public static string GeneratePasswordHash(string salt, string password, string hashAlgo)
        {
            return CryptoServices.ComputeHashBase64String(string.Concat(salt, password), hashAlgo);
        }

        /// <summary>
        /// Generates the password hash.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string GeneratePasswordHash(string salt, string password)
        {
            return GeneratePasswordHash(salt, password, "SHA512");
        }

        /// <summary>
        /// Matches the specified salt.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <param name="hashPassword">The hash password.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <returns></returns>
        public static bool Match(string salt, string password, string hashPassword, string hashAlgo)
        {
            return GeneratePasswordHash(salt, password, hashAlgo) == hashPassword;
        }

        /// <summary>
        /// Matches the specified salt.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <param name="hashPassword">The hash password.</param>
        /// <returns></returns>
        public static bool Match(string salt, string password, string hashPassword)
        {
            return GeneratePasswordHash(salt, password) == hashPassword;
        }
    }
}
