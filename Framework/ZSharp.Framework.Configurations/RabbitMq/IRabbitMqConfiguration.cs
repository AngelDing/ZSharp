namespace ZSharp.Framework.Configurations
{
    public interface IRabbitMqConfiguration
    {
        string VirtualHost { get; }

        string UserName { get; }

        string Password { get; }

        /// <summary>
        /// 消息发送后是否需要RabbitMq服务器回复确认信息
        /// </summary>
        bool PublisherConfirms { get; }

        /// <summary>
        /// 是否持久化消息
        /// </summary>
        bool PersistentMessages { get; }

        bool CancelOnHaFailover { get; }

        /// <summary>
        /// This flag tells the server how to react if the message cannot be routed to a queue. 
        /// If this flag is true, the server will return an unroutable message with a BasicReturn method. 
        /// If this flag is false, the server silently drops the message.
        /// </summary>
        bool Mandatory { get; }

        bool UseBackgroundThreads { get; }

        /// <summary>
        /// Heartbeat interval seconds. (default is 10)
        /// </summary>
        ushort RequestedHeartbeat { get; }

        // prefetchCount determines how many messages will be allowed in the local in-memory queue
        // setting to zero makes this infinite, but risks an out-of-memory exception, default is 50.
        // set to 50 based on this blog post:
        // http://www.rabbitmq.com/blog/2012/04/25/rabbitmq-performance-measurements-part-2/
        ushort PrefetchCount { get;  }

        /// <summary>
        /// Operation timeout seconds. (default is 10)
        /// </summary>
        ushort Timeout { get; }

        RabbitMqHostCollection RabbitMqHosts { get; }

        RabbitMqClientPropertyCollection RabbitMqClientProperties { get; }
    }
}