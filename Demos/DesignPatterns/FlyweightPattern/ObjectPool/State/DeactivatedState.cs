
namespace FlyweightPattern.ObjectPool
{
    /// <summary>
    /// ������ͷ�״̬
    /// </summary>
    public sealed class DeactivatedState : StateBase
    {
        #region Singleton
        public static DeactivatedState Instance;

        private DeactivatedState()
        {
        }

        static DeactivatedState()
        {
            Instance = new DeactivatedState();
        }
        #endregion

        public override bool Executable
        {
            get { return false; }
        }

        public override bool Unoccupied
        {
            get { return true; }
        }

        public override void Activate(IPoolable item)
        {
            item.ChangeState(ActivatedState.Instance);
        }

        public override void Deactivate(IPoolable item)
        {
        }
    }
}
