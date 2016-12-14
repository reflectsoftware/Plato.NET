using Newtonsoft.Json;
using Plato.Messaging.Implementations.RMQ;
using Plato.Messaging.Implementations.RMQ.Factories;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using Plato.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        static void ProducerConsumerTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);

            RMQQueueSettings queueSettings = _configurationManager.GetQueueSettings("TestQueue1");

            using (RMQProducerText producerText = new RMQProducerText(_connectionManager, "defaultConnection", queueSettings))
            {
                var sample = new
                {
                    Name = "Ross",
                    Location = "Toronto"
                };

                producerText.Send(JsonConvert.SerializeObject(sample));
            }

            using (RMQConsumerText consumerText = new RMQConsumerText(_connectionManager, "defaultConnection", queueSettings))
            {
                var result = consumerText.Receive();
                var data = result.Data;

                result.Acknowledge();
            }
        }

        static void PubSubTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);

            RMQQueueSettings queueSettings = _configurationManager.GetQueueSettings("TestQueue1");
        }
        
        static void Main(string[] args)
        {
            try
            {
                //ProducerConsumerTest();


                ////while (true)
                ////{
                ////    var key = Console.ReadKey();
                ////    if(key.KeyChar == 'q')
                ////    {
                ////        break;
                ////    }
                ////}

                //sender.Dispose();
                //_connectionManager.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
