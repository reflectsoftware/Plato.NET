using System.Threading;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQConsumerByte
    {
        RMQReceiverResultByte Receive(int msecTimeout = Timeout.Infinite);
    }
}
