using Plato.Messaging.Interfaces;
using System;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQPublisherText
    {
        void Send(string text, Action<ISenderProperties> action = null);
    }
}
