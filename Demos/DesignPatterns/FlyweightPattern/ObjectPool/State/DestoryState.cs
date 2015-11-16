
using System;

namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// 构造完成但没有被激活状态
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
