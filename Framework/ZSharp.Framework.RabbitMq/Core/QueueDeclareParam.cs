using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class QueueDeclareParam
    {
        public QueueDeclareParam(string name)
        {
            GuardHelper.ArgumentNotEmpty(() => name);
            Name = name;
            Durable = true;
        }

        /// <summary>
        /// The name of the queue
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Throw an exception rather than create the queue if it doesn't exist
        /// </summary>
        public bool Passive { get; set; }
        /// <summary>
        /// Durable queues remain active when a server restarts
        /// </summary>
        public bool Durable { get; set; }
        /// <summary>
        /// Exclusive queues may only be accessed by the current connection,
        /// and are deleted when that connection closes
        /// </summary>
        public bool Exclusive { get; set; }
        /// <summary>
        /// If set, the queue is deleted when all consumers have finished using it
        /// </summary>
        public bool AutoDelete { get; set; }
        /// <summary>
        /// Determines how long a message published to a queue can live before it is discarded by the server
        /// </summary>
        public int? PerQueueMessageTtl { get; set; }
        /// <summary>
        /// Determines how long a queue can remain unused before it is automatically deleted by the server
        /// </summary>
        public int? Expires { get; set; }
        /// <summary>
        /// Determines the maximum message priority that the queue should support
        /// </summary>
        public int? MaxPriority { get; set; }
        /// <summary>
        /// The maximum number of ready messages that may exist on the queue,
        /// Messages will be dropped or dead-lettered from the front of the queue to make room
        /// for new messages once the limit is reached
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// The maximum size of the queue in bytes. 
        /// Messages will be dropped or dead-lettered from the front of the queue to make room 
        /// for new messages once the limit is reached
        /// </summary>
        public int? MaxLengthBytes { get; set; }
        /// <summary>
        /// Determines an exchange's name can remain unused before it is automatically deleted by the server
        /// </summary>
        public string DeadLetterExchange { get; set; }
        /// <summary>
        /// If set, will route message with the routing key specified, if not set, 
        /// message will be routed with the same routing keys they were originally published with
        /// </summary>
        public string DeadLetterRoutingKey { get; set; }
    }
}
