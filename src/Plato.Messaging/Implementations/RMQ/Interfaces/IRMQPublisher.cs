using RabbitMQ.Client.Events;
using System;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQPublisher
    {
        Action<BasicReturnEventArgs> OnReturn { get; set; }
    }
}
