using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQSubscriberFactory
    {
        IRMQSubscriberByte CreateByte(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, IEnumerable<string> routingKeys = null);
        IRMQSubscriberByte CreateByte(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, string routingKey = "");
        IRMQSubscriberText CreateText(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, IEnumerable<string> routingKeys = null);
        IRMQSubscriberText CreateText(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, string routingKey = "");
    }
}
