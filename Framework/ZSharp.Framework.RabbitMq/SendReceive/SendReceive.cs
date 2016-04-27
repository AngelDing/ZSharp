using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class SendReceive : ISendReceive
    {
        private readonly IAdvancedBus advancedBus;
        private readonly IMessageDeliveryModeStrategy messageDeliveryModeStrategy;        
        private readonly ConcurrentDictionary<string, IQueue> declaredQueues; 

        public SendReceive(
            IAdvancedBus advancedBus,
            IMessageDeliveryModeStrategy messageDeliveryModeStrategy)
        {         
            this.advancedBus = advancedBus;
            this.messageDeliveryModeStrategy = messageDeliveryModeStrategy;
            declaredQueues = new ConcurrentDictionary<string, IQueue>();
        }

        public void Send<T>(string queue, T message) where T : class
        {            
            var wrappedMessage = PrepareSend(queue, message);
            advancedBus.Publish(Exchange.GetDefault(), queue, wrappedMessage);
        }

        public Task SendAsync<T>(string queue, T message) where T : class
        {
            var wrappedMessage = PrepareSend(queue, message);
            return advancedBus.PublishAsync(Exchange.GetDefault(), queue, wrappedMessage);
        }      

        public IDisposable Receive<T>(
            string queue, 
            Func<T, Task> onMessage,
            Action<IConsumerConfiguration> configure = null)
            where T : class
        {
            var declaredQueue = DeclareQueue(queue);
            return advancedBus.Consume<T>(declaredQueue, (message, info) => onMessage(message.Body), configure);
        }

        public IDisposable Receive(
            string queue,
            Action<IReceiveRegistration> addHandlers, 
            Action<IConsumerConfiguration> configure = null)
        {
            var declaredQueue = DeclareQueue(queue);
            return advancedBus.Consume(declaredQueue, x => addHandlers(new HandlerAdder(x)), configure);
        }

        private Message<T> PrepareSend<T>(string queue, T message) where T : class
        {
            GuardHelper.ArgumentNotEmpty(() => queue);
            GuardHelper.ArgumentNotNull(() => message);
            DeclareQueue(queue);
            var wrappedMessage = new Message<T>(message)
            {
                Properties =
                {
                    DeliveryMode = messageDeliveryModeStrategy.GetDeliveryMode(typeof(T))
                }
            };
            return wrappedMessage;
        }

        private IQueue DeclareQueue(string queueName)
        {
            IQueue queue = null;
            if (!declaredQueues.TryGetValue(queueName, out queue))
            {
                var param = new QueueDeclareParam(queueName);
                queue = advancedBus.QueueDeclare(param);
                declaredQueues.TryAdd(queueName, queue);
            }  
            return queue;
        }
    }
}