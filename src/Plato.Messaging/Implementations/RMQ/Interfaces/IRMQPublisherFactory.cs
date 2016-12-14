using Plato.Messaging.Implementations.RMQ.Settings;
using System.Collections.Generic;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQPublisherFactory
    {
        IRMQPublisherByte CreateByte(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, IEnumerable<string> routingKeys = null);
        IRMQPublisherByte CreateByte(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, string routingKey = "");
        IRMQPublisherText CreateText(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, IEnumerable<string> routingKeys = null);
        IRMQPublisherText CreateText(string connectionName, RMQExchangeSettings exchangeSettings, RMQQueueSettings queueSettings, string routingKey = "");
    }
}
