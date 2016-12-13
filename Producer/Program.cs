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
        static void Main(string[] args)
        {
            IRMQConfigurationManager _configurationManager = new RMQConfigurationManager();
            IRMQConnectionFactory _connectionManager = new RMQConnectionFactory(_configurationManager);

            using (var connection = _connectionManager.CreateConnection("defaultConnection"))
            {
            }
            
            //var esettings = _configurationManager.GetQueueSettings("SomeName1");

            //return;

            //var rmqSettings = new RMQSettings()
            //{
            //    ConnectionName = "defaultConnection",
            //    ConnectionFactory = _connectionFactory,
            //    ExchangeSettings  = _configurationManager.GetExchangeSettings("test")
            //};
                       

            //var senderFactory = new RMQSenderFactory();
            //var sender = senderFactory.CreateText(rmqSettings);

            //var sample =  new
            //{
            //    Name = "Ross",
            //    Location = "Toronto"
            //};

            //sender.Send( JsonConvert.SerializeObject(sample));

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
    }
}
