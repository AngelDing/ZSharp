/// <summary>
/// 外观模式：
/// 为子系统中的一组接口提供一个一致的界面，Facade 模式定义了一个高层接口，这个接口使得这一子系统更加容易使用。
/// 处理Legacy System比较有用；在API设计中也应该多采用此模式；
/// </summary>
namespace Common
{
    public interface IFacade
    {
        void Operation();
    }

    public class ConcreteFacade : IFacade
    {
        public void Operation()
        {
            // we could use any factory here
            // or use IoC here
            SubsystemClassA subsystemClassA = new SubsystemClassA();
            SubsystemClassB subsystemClassB = new SubsystemClassB();

            subsystemClassA.BehaviorA();
            subsystemClassB.BehaviorB();
        }
    }

    public class SubsystemClassA
    {
        public void BehaviorA()
        {
            // do something
        }
    }

    public class SubsystemClassB
    {
        public void BehaviorB()
        {
            // do something
        }
    }

    public class FacadeClient
    {
        public void TestCase1()
        {
            var facade = new ConcreteFacade();
            facade.Operation();
        }
    }
}
