
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// ������״̬����
    /// </summary>
    public abstract class StateBase : IState
    {
        /// <summary>
        /// ��ǰ״̬�£��ػ�ʵ���Ƿ����ִ��
        /// </summary>
        public abstract bool Executable { get; }

        /// <summary>
        /// ��ǰ״̬�£��ػ�ʵ���Ƿ������
        /// </summary>
        public abstract bool Unoccupied { get; }

        /// <summary>
        /// ����ʹ�ã���ʱ�ų���������Ը�ʵ����ʹ��
        /// </summary>
        /// <param name="item"></param>
        public abstract void Activate(IPoolable item);

        /// <summary>
        /// û�б��ͻ�����ռ�ã���ʱ����������Լ���ʹ�ø�ʵ��
        /// </summary>
        /// <param name="item"></param>
        public abstract void Deactivate(IPoolable item);

        /// <summary>
        /// �ͷ�, ��ʱ�������󲻿��Լ���ʹ�ø�ʵ��
        /// </summary>
        /// <param name="item"></param>
        public virtual void Dispose(IPoolable item)
        {
            item.ChangeState(DestoryState.Instance);
        }
    }
}