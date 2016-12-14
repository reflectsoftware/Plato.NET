using System.Threading;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQConsumerText
    {
        RMQReceiverResultText Receive(int msecTimeout = Timeout.Infinite);
    }
}
