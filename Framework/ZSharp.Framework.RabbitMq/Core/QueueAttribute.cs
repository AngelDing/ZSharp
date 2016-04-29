using System;

namespace ZSharp.Framework.RabbitMq
{
    /// <summary>
    /// 定義消息的路由器，隊列名稱前綴，默認採用消息的類型名稱
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false)]
    public class QueueAttribute : Attribute
    {
        public QueueAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        public string QueueName { get; private set; }

        public string ExchangeName { get; set; }
    }
}
