using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.RMQ;
using Plato.Messaging.RMQ.Factories;
using Plato.Messaging.RMQ.Settings;
using Plato.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        static private Task ProducerAsync()
        {
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "My.DLQ" }
            };

            var procuderFactory = new RMQProducerFactory(new RMQConnectionFactory());
            var connectionSettings = GetRMQConnectionSettings();
            var queueSettings = new RMQQueueSettings("My.Queue", "My.Queue", true, false, false, true, arguments: args);                        

            using (var producer = procuderFactory.CreateText(connectionSettings, queueSettings))
            {
                producer.Send("test1");
                producer.Send("test2");
                producer.Send("test3");
                producer.Send("test4");
            }

            return Task.CompletedTask;
        }

        static private Task ConsumerAsync()
        {
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "My.DLQ" }
            };

            var consumerFactory = new RMQConsumerFactory(new RMQConnectionFactory());
            var connectionSettings = GetRMQConnectionSettings();
            var queueSettings = new RMQQueueSettings("My.Queue", "My.Queue", true, false, false, true, arguments: args);

            using (var consumer = consumerFactory.CreateText(connectionSettings, queueSettings))
            {
                while (true)
                {
                    try
                    {
                        try
                        {
                            var message = consumer.Receive(1000);

                            //message.Reject(true);
                            //message.Reject();

                            message.Acknowledge();
                        }
                        catch (TimeoutException)
                        {
                        }
                        catch (MessageException ex)
                        {
                            switch (ex.ExceptionCode)
                            {
                                case MessageExceptionCode.ExclusiveLock:
                                    //await Task.Delay(5000);
                                    break;

                                case MessageExceptionCode.LostConnection:
                                    //await Task.Delay(5000);
                                    throw;

                                default:
                                    throw;
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (SQLErrors.IsSevereErrorCode(ex.Number))
                            {
                                // issue connecting with SQL server
                                //await Task.Delay(5000);
                            }

                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        consumer.ClearCacheBuffer();
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        static public async Task RunAsync()
        {
            // await ProducerAsync();
            await ConsumerAsync();
        }
    }
}
