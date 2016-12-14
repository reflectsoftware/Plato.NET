using Plato.Messaging.Implementations.RMQ.Settings;

namespace Plato.Messaging.Implementations.RMQ.Interfaces
{
    public interface IRMQProducerFactory
    {
        /// <summary>
        /// Creates the byte.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        IRMQProducerByte CreateByte(RMQQueueSettings settings, string connectionName);

        /// <summary>
        /// Creates the text.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        IRMQProducerText CreateText(RMQQueueSettings settings, string connectionName);
    }
}
