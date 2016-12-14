using System.Threading;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQSubscriberText
    {
        RMQReceiverResultText Receive(int msecTimeout = Timeout.Infinite);
    }
}
