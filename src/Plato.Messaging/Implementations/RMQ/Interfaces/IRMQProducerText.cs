using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQProducerText
    {
        void Send(string text, Action<ISenderProperties> action = null);
    }
}
