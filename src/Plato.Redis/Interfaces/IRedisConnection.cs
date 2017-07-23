using StackExchange.Redis;
using System;

namespace Plato.Redis.Interfaces
{
    public interface IRedisConnection: IDisposable
    {
        bool Disposed { get; }
        IConnectionMultiplexer Connection { get; }                        
        IDatabase GetDatabase(int db = -1, object asyncState = null, RedisDatabaseProxyConfiguration config = null);
    }
}