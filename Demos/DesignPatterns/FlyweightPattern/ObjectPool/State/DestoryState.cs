
using System;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// ������ɵ�û�б�����״̬
    /// </summary>
    public sealed class DestoryState : StateBase
    {
        #region Singleton
        public static DestoryState Instance;

        private DestoryState() 
        {
        }

        static DestoryState()
        {
            Instance = new DestoryState();
        }
        #endregion

        public override bool Executable
        {
            get { return false; }
        }

        public override bool Unoccupied
        {
            get { return false; }
        }

        public override void Activate(IPoolable item)
        {
            throw new NotSupportedException();
        }

        public override void Deactivate(IPoolable item)
        {
            throw new NotSupportedException();
        }
    }
}
