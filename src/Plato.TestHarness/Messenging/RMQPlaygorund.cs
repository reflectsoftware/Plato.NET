﻿using Plato.Messaging.Enums;
using Plato.Messaging.Exceptions;
using Plato.Messaging.RMQ;
using Plato.Messaging.RMQ.Factories;
using Plato.Messaging.RMQ.Settings;
using Plato.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plato.TestHarness.Messenging
{
    public class RMQPlayground
    {
        // http://zoltanaltfatter.com/2016/09/06/dead-letter-queue-configuration-rabbitmq/

        static RMQConfigurationManager CreateConfigurationManager()
        {
            var connectionSettings = new RMQConnectionSettings
            {
                Name = "defaultConnection",
                Username = "local-dev-user",
                Password = "local-dev-user",
                VirtualHost = "local-dev-vh",
                Uri = "amqp://localhost:5672,amqp://localhost:5673,amqp://localhost:5674",
                DelayOnReconnect = 2000,
            };

            var queueSettings = new RMQQueueSettings("MY_RMQ_TEST", "MY_RMQ_TEST")
            {                
                Durable = true,
                AutoDelete = false,
                Persistent = true,
                Exclusive = false,                       
            };

            return new RMQConfigurationManager(new[] { connectionSettings }, queueSettings: new[] { queueSettings });
        }             

        static private Task ProducerAsync()
        {
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "DLQ.MY_RMQ_TEST" }
            };

            var configManager = CreateConfigurationManager();            
            var procuderFactory = new RMQProducerFactory(new RMQConnectionFactory());
            var connectionSettings = configManager.GetConnectionSettings("defaultConnection");
            var queueSettings = configManager.GetQueueSettings("MY_RMQ_TEST", args); 

            using (var producer = procuderFactory.CreateText(connectionSettings, queueSettings))
            {
                producer.Send("test1");
                producer.Send("test2");
                producer.Send("test3");
                producer.Send("test4");
            }

            return Task.CompletedTask;
        }

        static private async Task ProducerPerformanceTestAsync()
        {
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "My.DLQ" }
            };

            var configManager = CreateConfigurationManager();            
            var procuderFactory = new RMQProducerFactory(new RMQConnectionFactory());
            var connectionSettings = configManager.GetConnectionSettings("defaultConnection");
            var queueSettings = configManager.GetQueueSettings("MY_RMQ_TEST", args);

            using (var producer = procuderFactory.CreateText(connectionSettings, queueSettings))
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Provide a command: quit, clear or a number");

                        var command = Console.ReadLine();
                        if (command == "quit" || command == "exit")
                        {
                            break;
                        }

                        if (command == "clear")
                        {
                            Console.Clear();
                            continue;
                        }

                        if (!int.TryParse(command, out int count) || count < 0)
                        {
                            Console.WriteLine("Invalid iteration number");
                            continue;
                        }

                        var sw = new Stopwatch();
                        sw.Reset();
                        sw.Start();

                        for (var i = 0; i < count; i++)
                        {
                            var data = $"Message from Ross: {i}";
                            await producer.SendAsync(data);
                        }

                        sw.Stop();
                        Console.WriteLine($"Done sending: {sw.ElapsedMilliseconds}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        static private Task ConsumerAsync()
        {
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", "My.DLQ" }
            };

            var configManager = CreateConfigurationManager();
            var consumerFactory = new RMQConsumerFactory(new RMQConnectionFactory());
            var connectionSettings = configManager.GetConnectionSettings("defaultConnection");
            var queueSettings = configManager.GetQueueSettings("MY_RMQ_TEST", args);

            using (var consumer = consumerFactory.CreateText(connectionSettings, queueSettings))
            {
                consumer.Mode = ConsumerMode.OnNoMessage_ReturnNull;

                while (true)
                {
                    try
                    {
                        try
                        {
                            var message = consumer.Receive(1000);
                            if (message != null)
                            {
                                //message.Reject(true);
                                //message.Reject();

                                ReflectSoftware.Insight.GReflectInsight.SendMessage(message.Data);
                                message.Acknowledge();
                            }
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
                                    break;

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
            // await ProducerPerformanceTestAsync();
            await ProducerAsync();
            //await ConsumerAsync();
        }
    }
}
