using StackExchange.Redis;
using System;
using System.Threading;

namespace Plato.Redis.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisControl
    {
        /// <summary>
        /// Gets the redis database.
        /// </summary>
        /// <value>
        /// The redis database.
        /// </value>
        IDatabase RedisDb { get; }

        /// <summary>
        /// Gets the redis key.
        /// </summary>
        /// <value>
        /// The redis key.
        /// </value>
        string RedisKey { get; }

        /// <summary>
        /// Locks the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="lockLength">Length of the lock.</param>
        /// <returns></returns>
        bool Lock(int timeout = Timeout.Infinite, TimeSpan? lockLength = null);

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        void Unlock();
    }
}
