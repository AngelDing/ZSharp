using ZSharp.Framework.Utils;

namespace ZSharp.Framework.RabbitMq
{
    public class ExchangeDeclareParam
    {
        public ExchangeDeclareParam(string name, string type)
        {
            GuardHelper.CheckStringLength(() => name, 255);
            GuardHelper.CheckStringLength(() => type, 255);
            Name = name;
            Type = type;
            Durable = true;
        }

        /// <summary>
        /// The exchange name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The type of exchange
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Route messages to this exchange if they cannot be routed
        /// </summary>
        public string AlternateExchange { get; set; }
        /// <summary>
        /// Throw an exception rather than create the exchange if it doens't exist
        /// </summary>
        public bool Passive { get; set; }
        /// <summary>
        /// Durable exchanges remain active when a server restarts
        /// </summary>
        public bool Durable { get; set; }
        /// <summary>
        /// If set, the exchange is deleted when all queues have finished using it
        /// </summary>
        public bool AutoDelete { get; set; }
        /// <summary>
        /// If set, the exchange may not be used directly by publishers, but only when bound to other exchanges
        /// </summary>
        public bool Internal { get; set; }
        /// <summary>
        /// If set, declars x-delayed-type exchange for routing delayed messages
        /// </summary>
        public bool Delayed { get; set; }
    }
}
