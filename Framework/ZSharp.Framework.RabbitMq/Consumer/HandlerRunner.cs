using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Exceptions;

namespace ZSharp.Framework.RabbitMq
{
    public interface IHandlerRunner : IDisposable
    {
        void InvokeUserMessageHandler(ConsumerExecutionContext context);
    }

    public class HandlerRunner : BaseRabbitMq, IHandlerRunner
    {
        private readonly IConsumerErrorStrategy consumerErrorStrategy;

        public HandlerRunner(IConsumerErrorStrategy consumerErrorStrategy)
        {
            this.consumerErrorStrategy = consumerErrorStrategy;
        }

        public void InvokeUserMessageHandler(ConsumerExecutionContext context)
        {
            Logger.Debug("Received \n\tRoutingKey: '{0}'\n\tCorrelationId: '{1}'\n\tConsumerTag: '{2}'" +
                "\n\tDeliveryTag: {3}\n\tRedelivered: {4}",
                context.Info.RoutingKey,
                context.Properties.CorrelationId,
                context.Info.ConsumerTag,
                context.Info.DeliverTag,
                context.Info.Redelivered);

            Task completionTask;
            
            try
            {
                completionTask = context.UserHandler(context.Body, context.Properties, context.Info);
            }
            catch (Exception exception)
            {
                completionTask = TaskHelpers.FromException(exception);
            }
            
            if (completionTask.Status == TaskStatus.Created)
            {
                Logger.Error("Task returned from consumer callback is not started. ConsumerTag: '{0}'",
                    context.Info.ConsumerTag);
                return;
            }
            
            completionTask.ContinueWith(task => DoAck(context, GetAckStrategy(context, task)));
        }

        private AckStrategy GetAckStrategy(ConsumerExecutionContext context, Task task)
        {
            var ackStrategy = AckStrategies.Ack;
            try
            {
                if (task.IsFaulted)
                {
                    Logger.Error(BuildErrorMessage(context, task.Exception));
                    ackStrategy = consumerErrorStrategy.HandleConsumerError(context, task.Exception);
                }
                else if (task.IsCanceled)
                {
                    ackStrategy = consumerErrorStrategy.HandleConsumerCancelled(context);
                }
            }
            catch (Exception consumerErrorStrategyError)
            {
                Logger.Error("Exception in ConsumerErrorStrategy:\n{0}", consumerErrorStrategyError);
                return AckStrategies.Nothing;
            }
            return ackStrategy;
        }
        
        private void DoAck(ConsumerExecutionContext context, AckStrategy ackStrategy)
        {
            const string failedToAckMessage =
                "Basic ack failed because channel was closed with message '{0}'." +
                " Message remains on RabbitMQ and will be retried." +
                " ConsumerTag: {1}, DeliveryTag: {2}";

            var ackResult = AckResult.Exception;
            var deliverTag = context.Info.DeliverTag;
            try
            {
                ackResult = ackStrategy(context.Consumer.Model, deliverTag);
            }
            catch (AlreadyClosedException alreadyClosedException)
            {
                Logger.Info(failedToAckMessage, alreadyClosedException.Message, context.Info.ConsumerTag, deliverTag);
            }
            catch (IOException ioException)
            {
                Logger.Info(failedToAckMessage, ioException.Message, context.Info.ConsumerTag, deliverTag);
            }
            catch (Exception exception)
            {
                Logger.Error("Unexpected exception when attempting to ACK or NACK\n{0}", exception);
            }
            finally
            {
                EventBus.Publish(new AckEvent(context.Info, context.Properties, context.Body, ackResult));
            }
        }

        private string BuildErrorMessage(ConsumerExecutionContext context, Exception exception)
        {
            var message = Encoding.UTF8.GetString(context.Body);

            return "Exception thrown by subscription callback.\n" +
                   string.Format("\tExchange:    '{0}'\n", context.Info.Exchange) +
                   string.Format("\tRouting Key: '{0}'\n", context.Info.RoutingKey) +
                   string.Format("\tRedelivered: '{0}'\n", context.Info.Redelivered) +
                   string.Format("Message:\n{0}\n", message) +
                   string.Format("BasicProperties:\n{0}\n", context.Properties) +
                   string.Format("Exception:\n{0}\n", exception);
        }

        public void Dispose()
        {
            consumerErrorStrategy.Dispose();
        }
    }

}