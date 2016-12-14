using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQProducerByte
    {
        void Send(byte[] data, Action<ISenderProperties> action = null);
    }
}
