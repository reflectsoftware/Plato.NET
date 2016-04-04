// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public class CryptoNonce
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public byte[] Key { get; set; }

        /// <summary>
        /// Gets or sets the iv.
        /// </summary>
        /// <value>
        /// The iv.
        /// </value>
        public byte[] IV { get; set; }
    }
}
