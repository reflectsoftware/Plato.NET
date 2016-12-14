using System.Threading;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQSubscriberByte
    {
        RMQReceiverResultByte Receive(int msecTimeout = Timeout.Infinite);
    }
}
