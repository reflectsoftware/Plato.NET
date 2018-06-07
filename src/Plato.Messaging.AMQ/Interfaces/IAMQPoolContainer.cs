using System;
using Plato.Messaging.Interfaces;

namespace Plato.Messaging.AMQ.Interfaces
{
    public interface IAMQPoolContainer<T> : IDisposable where T : IMessageReceiverSender
    {
        T Instance { get; }
        Guid PoolId { get; }
    }
}