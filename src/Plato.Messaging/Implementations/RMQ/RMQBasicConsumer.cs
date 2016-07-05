// Plato.NET
// Copyright (c) 2016 ReflectSoftware Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Util;

namespace Plato.Messaging.Implementations.RMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="RabbitMQ.Client.DefaultBasicConsumer" />
    /// <seealso cref="RabbitMQ.Client.IQueueingBasicConsumer" />
    public class RMQBasicConsumer : DefaultBasicConsumer, IQueueingBasicConsumer
    {
        /// <summary>
        /// Creates a fresh <see cref="RMQBasicConsumer" />,
        /// initializing the <see cref="DefaultBasicConsumer.Model" /> property to null
        /// and the <see cref="Queue" /> property to a fresh <see cref="SharedQueue" />.
        /// </summary>
        public RMQBasicConsumer() : this(null)
        {
        }

        /// <summary> 
        /// Creates a fresh <see cref="RMQBasicConsumer"/>, with <see cref="DefaultBasicConsumer.Model"/> 
        ///  set to the argument, and <see cref="Queue"/> set to a fresh <see cref="SharedQueue"/>. 
        /// </summary> 
        public RMQBasicConsumer(IModel model) : this(model, new SharedQueue<BasicDeliverEventArgs>())
        {
        }

        /// <summary> 
        /// Creates a fresh <see cref="RMQBasicConsumer"/>, 
        ///  initializing the <see cref="DefaultBasicConsumer.Model"/> 
        ///  and <see cref="Queue"/> properties to the given values. 
        /// </summary> 
        public RMQBasicConsumer(IModel model, SharedQueue<BasicDeliverEventArgs> queue) : base(model)
        {
            Queue = queue;
        }

        /// <summary> 
        /// Retrieves the <see cref="SharedQueue"/> that messages arrive on. 
        /// </summary> 
        public SharedQueue<BasicDeliverEventArgs> Queue { get; protected set; }
        
        /// <summary> 
        /// Overrides <see cref="DefaultBasicConsumer"/>'s  <see cref="HandleBasicDeliver"/> implementation, 
        ///  building a <see cref="BasicDeliverEventArgs"/> instance and placing it in the Queue. 
        /// </summary> 
        public override void HandleBasicDeliver(string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            byte[] body)
        {
            var eventArgs = new BasicDeliverEventArgs
            {
                ConsumerTag = consumerTag,
                DeliveryTag = deliveryTag,
                Redelivered = redelivered,
                Exchange = exchange,
                RoutingKey = routingKey,
                BasicProperties = properties,
                Body = body
            };

            Queue.Enqueue(eventArgs);
        }

        /// <summary> 
        /// Overrides <see cref="DefaultBasicConsumer"/>'s OnCancel implementation, 
        ///  extending it to call the Close() method of the <see cref="SharedQueue"/>. 
        /// </summary> 
        public override void OnCancel()
        {
            base.OnCancel();
            Queue.Close();
        }
    }
}


