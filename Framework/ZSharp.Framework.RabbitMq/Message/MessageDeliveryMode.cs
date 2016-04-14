
namespace ZSharp.Framework.RabbitMq
{
    public static class MessageDeliveryMode
    {
        /// <summary>
        /// 瞬时，短暂
        /// </summary>
        public const byte Transient = 1;

        /// <summary>
        /// 持久
        /// </summary>
        public const byte Persistent = 2;
    }
}
