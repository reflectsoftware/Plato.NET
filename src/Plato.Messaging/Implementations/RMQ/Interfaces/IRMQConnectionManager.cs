using Plato.Messaging.Interfaces;
using RabbitMQ.Client;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQConnectionManager : IMessageConnectionManager<IConnection>
    {
    }
}
