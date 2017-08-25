using Plato.Messaging.RMQ;
using Plato.Messaging.RMQ.Factories;
using Plato.Messaging.RMQ.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plato.TestHarness.RMQ
{
    public class RMQPlayground
    {
        // http://zoltanaltfatter.com/2016/09/06/dead-letter-queue-configuration-rabbitmq/

        static private RMQConnectionSettings GetRMQConnectionSettings()
        {
            var connectionSettings = new RMQConnectionSettings
            {
                Name = "defaultConnection",
                Username = "local-dev-user",
                Password = "local-dev-user",
                VirtualHost = "local-dev-vh",
                Uri = "amqp://localhost:5673",
                DelayOnReconnect = 1000,
                ForceReconnectionTime = TimeSpan.FromMinutes(6),
            };

            // prepare endpoints
            foreach (var uri in connectionSettings.Uri.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                connectionSettings.Endpoints.Add(uri);
            }

            return connectionSettings;
        }

        static private void Producer()
        {
            //var xxx = new RMQConfigurationManager();
            //var qs = xxx.GetQueueSettings("My.Queue");
            
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "My.DLQ" }
            };

            var procuderFactory = new RMQProducerFactory(new RMQConnectionFactory());
            var connectionSettings = GetRMQConnectionSettings();
            var queueSettings = new RMQQueueSettings("My.Queue", "My.Queue1", true, false, false, true, arguments: args);                        

            using (var producer = procuderFactory.CreateText(connectionSettings, queueSettings))
            {
                producer.Send("test1");
                producer.Send("test2");
                producer.Send("test3");
                producer.Send("test4");
            }
        }

        static private void Consumer()
        {
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "My.DLQ" }
            };

            var consumerFactory = new RMQConsumerFactory(new RMQConnectionFactory());
            var connectionSettings = GetRMQConnectionSettings();
            var queueSettings = new RMQQueueSettings("My.Queue", "My.Queue2", true, false, false, true, arguments: args);

            using (var consumer = consumerFactory.CreateText(connectionSettings, queueSettings))
            {
                while (true)
                {
                    try
                    {
                        var message = consumer.Receive(1000);

                        message.Reject(true);

                        message.Reject();

                        message.Acknowledge();

                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }


        static public Task RunAsync()
        {
            // Producer();
            // Consumer();

            return Task.CompletedTask;
        }
    }
}
