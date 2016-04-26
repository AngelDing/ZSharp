using System;

namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// A wrapper for errored messages
    /// </summary>
    public class ErrorMessage
    {
        public string RoutingKey { get; set; }
        public string Exchange { get; set; }
        public string Exception { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public MessageProperties BasicProperties { get; set; }
    }
}