// Plato.NET
// Copyright (c) 2017 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace Plato.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisDatabaseProxyConfiguration
    {
        /// <summary>
        /// Gets the retry.
        /// </summary>
        /// <value>
        /// The retry.
        /// </value>
        public int Retry { get; }

        /// <summary>
        /// Gets the retry wait.
        /// </summary>
        /// <value>
        /// The retry wait.
        /// </value>
        public int RetryWait { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisDatabaseProxyConfiguration"/> class.
        /// </summary>
        /// <param name="retry">The retry.</param>
        /// <param name="retryWait">The retry wait.</param>
        public RedisDatabaseProxyConfiguration(int retry = 5, int retryWait = 1000)
        {
            Retry = retry;
            RetryWait = retryWait;
        }
    }
}
