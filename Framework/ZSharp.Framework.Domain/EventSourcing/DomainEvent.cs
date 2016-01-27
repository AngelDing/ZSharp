
using System;

namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// ��ʾһ�������¼���Ϣ�����������¼����������¼���Դ
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// �����¼��汾
        /// </summary>
        int Version { get; }

        /// <summary>
        /// �������¼���Դ����/�¼���һ��ָԴ����Id
        /// </summary>
        Guid CorrelationId { get; set; }
    }

    public abstract class DomainEvent : Event, IDomainEvent
    {        
        public Guid CorrelationId { get; set; }

        public int Version { get; set; }
    }
}