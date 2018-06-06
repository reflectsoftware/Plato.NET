using Plato.Messaging.AMQ.Factories;
using Plato.Messaging.AMQ.Settings;
using Plato.Messaging.Enums;
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
using Apache.NMS;

namespace Plato.TestHarness.Messenging
{
    public class AMQPlayground
    {
        static private AMQConnectionSettings GetRMQConnectionSettings()
        {
            var connectionSettings = new AMQConnectionSettings
            {
                Name = "defaultConnection",
                Username = "admin",
                Password = "admin",                
                Uri = "tcp://localhost:61616?tcpNoDelay=true,tcp://localhost:61616?tcpNoDelay=true",
                DelayOnReconnect = 1000,                
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
            var connectionSettings = GetRMQConnectionSettings();
            var senderFactory = new AMQSenderFactory(new AMQConnectionFactory());
            var queueSettings = new AMQDestinationSettings("MY_AMQ_TEST", "queue://MY_AMQ_TEST")
            {
                DeliveryMode = MsgDeliveryMode.Persistent,
                Durable = true,
            };

            using (var sender = senderFactory.CreateText(connectionSettings, queueSettings))
            {
                sender.Send("test1");
                sender.Send("test2");
                sender.Send("test3");
                sender.Send("test4");
            }

            return Task.CompletedTask;
        }

        static private async Task ProducerPerformanceTestAsync()
        {
            var connectionSettings = GetRMQConnectionSettings();
            var senderFactory = new AMQSenderFactory(new AMQConnectionFactory());
            var queueSettings = new AMQDestinationSettings("MY_AMQ_TEST", "queue://MY_AMQ_TEST")
            {
                DeliveryMode = MsgDeliveryMode.Persistent,
                Durable = true,
            };

            using (var producer = senderFactory.CreateText(connectionSettings, queueSettings))
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
            var connectionSettings = GetRMQConnectionSettings();
            var receiverFactory = new AMQReceiverFactory(new AMQConnectionFactory());
            var queueSettings = new AMQDestinationSettings("MY_AMQ_TEST", "queue://MY_AMQ_TEST")
            {
                DeliveryMode = MsgDeliveryMode.Persistent,
                Durable = true,
            };

            using (var receiver = receiverFactory.CreateText(connectionSettings, queueSettings))
            {
                while (true)
                {
                    try
                    {
                        try
                        {
                            var message = receiver.Receive(1000);
                            if (message != null)
                            {
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
                                    // await Task.Delay(5000);
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
                        receiver.ClearCacheBuffer();
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        static public async Task RunAsync()
        {
            // await ProducerPerformanceTestAsync();

            //await ProducerAsync();
            await ConsumerAsync();
        }
    }
}
