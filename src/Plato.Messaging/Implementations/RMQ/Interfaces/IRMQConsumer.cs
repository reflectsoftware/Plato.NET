using System;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQConsumer
    {
        void ClearCacheBuffer();
    }
}
