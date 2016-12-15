using Newtonsoft.Json;
using Plato.Messaging.Implementations.RMQ;
using Plato.Messaging.Implementations.RMQ.Factories;
using Plato.Messaging.Implementations.RMQ.Interfaces;
using Plato.Messaging.Implementations.RMQ.Settings;
using ReflectSoftware.Insight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    class SampleData
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }

    class Program
    {
        static void Reciever(IRMQReceiverText receiver)
        {
            Console.WriteLine("Ready!");

            while (true)
            {
                try
                {
                    var result = receiver.Receive(1000);
                    result.Acknowledge();

                    var data = result.Data;

                    if (data == "clear")
                    {
                        RILogManager.Default.ViewerClearAll();
                        continue;
                    }
                    else if (data == "stop")
                    {
                        break;
                    }

                    var sample = JsonConvert.DeserializeObject<SampleData>(data);
                    RILogManager.Default.SendMessage(sample.Name);
                }
                catch (TimeoutException)
                {
                }
                catch (Exception ex)
                {
                    RILogManager.Default.SendException(ex);
                    Thread.Sleep(2000);
                }
            }
        }

        static void ConsumerTest()
        {            
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);
            RMQQueueSettings queueSettings = _configurationManager.GetQueueSettings("ProConQueueTest");

            using (IRMQConsumerText consumerText = new RMQConsumerText(_connectionManager, "defaultConnection", queueSettings))
            {
                Reciever(consumerText);
            }
        }

        static void SubscribeTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);

            var exchangeSettings = _configurationManager.GetExchangeSettings("Test.FanoutExchange");
            var queueSettings = _configurationManager.GetQueueSettings("Test.FanoutQueue");

            using (IRMQSubscriberText consumerText = new RMQSubscriberText(_connectionManager, "defaultConnection", exchangeSettings, queueSettings))
            {
                Reciever(consumerText);
            }
        }

        static void SubscribeDirectTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);

            var exchangeSettings = _configurationManager.GetExchangeSettings("Test.DirectExchange");
            var queueSettings = _configurationManager.GetQueueSettings("Test.DirectQueue");

            using (IRMQSubscriberText consumerText = new RMQSubscriberText(_connectionManager, "defaultConnection", exchangeSettings, queueSettings))
            {
                Reciever(consumerText);
            }
        }
        
        static void Main(string[] args)
        {
            try
            {
                //ConsumerTest();
                //SubscribeTest();
                SubscribeDirectTest();
            }
            catch(Exception ex)
            {
                RILogManager.Default.SendException(ex);
            }
        }
    }
}
