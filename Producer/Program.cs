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
    class SampleData
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }

    class Program
    {
        static void Send(IMessageSender<string> sender, SampleData sample, List<string> routes = null)
        {
            var data = JsonConvert.SerializeObject(sample);
            if (routes == null)
            {
                sender.Send(data);
            }
            else
            {
                var rnd = new Random((int)DateTime.Now.Ticks);
                var route = routes[rnd.Next(routes.Count)];
                Console.WriteLine($"Route: {route}");

                sender.Send(data, (props) =>
                {
                    var senderProps = (RMQSenderProperties)props;
                    senderProps.RoutingKey = route;
                });
            }
        }

        static void Sender(IMessageSender<string> sender, List<string> routes = null)
        {
            Console.WriteLine("Ready!");                       

            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 's')
                {
                    sender.Send("stop");
                    break;
                }
                else if (key.KeyChar == 'c')
                {
                    sender.Send("clear");                    
                }
                else if (key.KeyChar == '1') // single
                {
                    var sample = new SampleData
                    {
                        Name = "Ross",
                        Location = "Toronto"
                    };

                    Send(sender, sample, routes);

                }
                else if (key.KeyChar == '2' || key.KeyChar == '3') 
                {
                    var count = key.KeyChar == '3' ? 10000 : 1000;
                    for (var i = 0; i < count; i++)
                    {
                        var sample = new SampleData
                        {
                            Name = $"Ross: {i}",
                            Location = "Toronto"
                        };

                        Send(sender, sample, routes);
                    }
                }
                else
                {
                    Console.WriteLine("Unknown action");
                    continue;
                }

                Console.WriteLine("Done!");
            }
        }

        static void ProducerTest()
        {            
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);
            var queueSettings = _configurationManager.GetQueueSettings("ProConQueueTest");

            using (IRMQProducerText producerText = new RMQProducerText(_connectionManager, "defaultConnection", queueSettings))
            {
                Sender(producerText);
            }
        }

        static void SubscribeFanoutTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);
            var exchangeSettings = _configurationManager.GetExchangeSettings("Test.FanoutExchange");

            using (IRMQPublisherText publisherText = new RMQPublisherText(_connectionManager, "defaultConnection", exchangeSettings))
            {
                Sender(publisherText);
            }
        }

        static void PubSubDirectTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);
            var exchangeSettings = _configurationManager.GetExchangeSettings("Test.DirectExchange");

            using (IRMQPublisherText publisherText = new RMQPublisherText(_connectionManager, "defaultConnection", exchangeSettings))
            {
                Sender(publisherText, new List<string> { "R1", "R2", "R3"});
            }
        }

        static void PubSubTopicTest()
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);
            var exchangeSettings = _configurationManager.GetExchangeSettings("Test.TopicExchange");

            using (IRMQPublisherText publisherText = new RMQPublisherText(_connectionManager, "defaultConnection", exchangeSettings))
            {
                Console.WriteLine("Press any key...");
                Console.ReadKey();
                
                publisherText.Send("{'Name':'fast.orange.dog'}", (props) =>
                {
                    var senderProps = (RMQSenderProperties)props;
                    senderProps.RoutingKey = "fast.orange.dog";
                    Console.WriteLine(senderProps.RoutingKey);
                });


                Console.WriteLine("Press any key...");
                Console.ReadKey();

                publisherText.Send("{'Name':'fast.brown.rabbit'}", (props) =>
                {
                    var senderProps = (RMQSenderProperties)props;
                    senderProps.RoutingKey = "fast.brown.rabbit";
                    Console.WriteLine(senderProps.RoutingKey);
                });

                Console.WriteLine("Press any key...");
                Console.ReadKey();

                publisherText.Send("{'Name':'lazy.orange.dog'}", (props) =>
                {
                    var senderProps = (RMQSenderProperties)props;
                    senderProps.RoutingKey = "lazy.orange.dog";
                    Console.WriteLine(senderProps.RoutingKey);
                });

                Console.WriteLine("Press any key...");
                Console.ReadKey();

                publisherText.Send("{'Name':'fast.brown.dog'}", (props) =>
                {
                    var senderProps = (RMQSenderProperties)props;
                    senderProps.RoutingKey = "fast.brown.dog";
                    Console.WriteLine(senderProps.RoutingKey);
                });
            }
        }
        
        static void Main(string[] args)
        {
            try
            {
                //ProducerTest();
                //SubscribeFanoutTest();
                //PubSubDirectTest();
                PubSubTopicTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
