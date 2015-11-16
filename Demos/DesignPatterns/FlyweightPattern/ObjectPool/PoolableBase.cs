
using System;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// �����ĳ���ɳػ�����
    /// <remarks>
    ///     ������ʵ�����Ա����ã��������ϲ�֧��������״̬��Ϣ
    /// </remarks>
    /// </summary>
    public abstract class PoolableBase : IPoolable
    {
        #region Constructor
        /// <summary>
        /// ����ػ�����Ļ���׼������
        /// </summary>
        public PoolableBase()
        {
            this.Guid = System.Guid.NewGuid().ToString();
            this.Type = GetType();
            this.CreateTime = DateTime.Now;
            this.AccessedTime = DateTime.Now;
            this.State = ConstructedState.Instance;
        }
        #endregion

        #region Public Property
        /// <summary>
        /// ��ǰʵ�����ڴ��Ψһ��ʶ
        /// </summary>
        public string Guid { get; private set; }

        /// <summary>
        /// ʵ������
        /// </summary>
        public virtual Type Type { get; private set; }

        /// <summary>
        /// ��ǰʵ������ʱ��
        /// </summary>
        public virtual DateTime CreateTime { get; private set; }

        /// <summary>
        /// ���һ�α�����ʱ��
        /// </summary>
        public virtual DateTime AccessedTime { get; private set; }

        /// <summary>
        /// ��ǰʵ��������״̬
        /// </summary>
        public virtual IState State { get; private set; }

        #endregion

        #region State Management

        /// <summary>
        /// ��ǰʵ���Ƿ����ִ����Ӧ�Ĺ���
        /// </summary>
        public bool Executable
        {
            get { return State.Executable; }
        }

        /// <summary>
        /// �ػ�ʵ���Ƿ������
        /// </summary>
        public bool Unoccupied
        {
            get { return State.Unoccupied; }
        }

        /// <summary>
        /// �޸ĵ�ǰ�ĳػ�ʵ����״̬
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IState newState)
        {
            this.State = newState;
        }

        /// <summary>
        /// ����ʹ�ã���ʱ�ų���������Ը�ʵ����ʹ��
        /// </summary>
        public virtual void Activate()
        {
            State.Activate(this);
        }

        /// <summary>
        /// û�б��ͻ�����ռ�ã���ʱ����������Լ���ʹ�ø�ʵ��
        /// </summary>
        public virtual void Deactivate()
        {
            State.Deactivate(this);
        }

        /// <summary>
        /// �ͷ�, ��ʱ�������󲻿��Լ���ʹ�ø�ʵ��
        /// </summary>
        public virtual void Dispose()
        {
            State.Dispose(this);
        }
        #endregion

        #region Helper method
        /// <summary>
        /// �������������ʱ��
        /// </summary>
        protected void PreProcess()
        {
            if (!Executable)
            {
                throw new NotSupportedException();
            }
            this.AccessedTime = DateTime.Now;
        }
        #endregion
    }
}
