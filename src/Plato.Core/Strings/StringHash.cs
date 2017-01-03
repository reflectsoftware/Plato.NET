// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Core.Strings
{
    /// <summary>
    ///
    /// </summary>
    public static class StringHash
    {
        /// <summary>
        /// Rses the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long RSHash(string str)
        {
            const int b = 378551;
            var a = 63689;
            var hash = 0L;

            for (var i = 0; i < str.Length; i++)
            {
                hash = hash * a + str[i];
                a = a * b;
            }

            return hash;
        }

        /// <summary>
        /// Jses the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long JSHash(string str)
        {
            var hash = 1315423911L;

            for (var i = 0; i < str.Length; i++)
            {
                hash ^= ((hash << 5) + str[i] + (hash >> 2));
            }

            return hash;
        }

        /// <summary>
        /// Elfs the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long ELFHash(string str)
        {
            long hash = 0;
            var x = 0L;

            for (var i = 0; i < str.Length; i++)
            {
                hash = (hash << 4) + str[i];

                if ((x = hash & 0xF0000000L) != 0)
                {
                    hash ^= (x >> 24);
                }
                hash &= ~x;
            }

            return hash;
        }

        /// <summary>
        /// BKDRs the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long BKDRHash(string str)
        {
            const long seed = 131;
            var hash = 0L;

            for (var i = 0; i < str.Length; i++)
            {
                hash = (hash * seed) + str[i];
            }

            return hash;
        }

        /// <summary>
        /// SDBMs the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long SDBMHash(string str)
        {
            var hash = 0L;

            for (var i = 0; i < str.Length; i++)
            {
                hash = str[i] + (hash << 6) + (hash << 16) - hash;
            }

            return hash;
        }

        /// <summary>
        /// DJBs the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long DJBHash(string str)
        {
            var hash = 5381L;

            for (var i = 0; i < str.Length; i++)
            {
                hash = ((hash << 5) + hash) + str[i];
            }

            return hash;
        }

        /// <summary>
        /// Deks the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long DEKHash(string str)
        {
            long hash = str.Length;

            for (var i = 0; i < str.Length; i++)
            {
                hash = ((hash << 5) ^ (hash >> 27)) ^ str[i];
            }

            return hash;
        }

        /// <summary>
        /// Bps the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long BPHash(string str)
        {
            var hash = 0L;

            for (var i = 0; i < str.Length; i++)
            {
                hash = hash << 7 ^ str[i];
            }

            return hash;
        }

        /// <summary>
        /// FNVs the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long FNVHash(string str)
        {
            long fnv_prime = 0x811C9DC5;
            long hash = 0;

            for (var i = 0; i < str.Length; i++)
            {
                hash *= fnv_prime;
                hash ^= str[i];
            }

            return hash;
        }

        /// <summary>
        /// Aps the hash.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static long APHash(string str)
        {
            long hash = 0xAAAAAAAA;

            for (var i = 0; i < str.Length; i++)
            {
                if ((i & 1) == 0)
                {
                    hash ^= ((hash << 7) ^ str[i] * (hash >> 3));
                }
                else
                {
                    hash ^= (~((hash << 11) + str[i] ^ (hash >> 5)));
                }
            }

            return hash;
        }
    }
}
