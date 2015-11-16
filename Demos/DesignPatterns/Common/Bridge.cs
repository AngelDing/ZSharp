/// <summary>
/// 桥接模式 ：　将抽象部分与它的实现部分分离，使它们都可以独立地变化；同Adapter結合一起使用；
/// </summary>
namespace Common
{
    public interface IImpl
    {
        void OperationImpl();
    }

    public interface IAbstraction
    {
        IImpl Implementor { get; }

        void Operation();
    }

    public class ConcreteImplementatorA : IImpl { public void OperationImpl() { } }

    public class ConcreteImplementatorB : IImpl { public void OperationImpl() { } }

    public class RefinedAbstraction : IAbstraction
    {
        /// <summary>
        /// 構造器注入
        /// </summary>
        /// <param name="implementor"></param>
        public RefinedAbstraction(IImpl implementor)
        {
            this.Implementor = implementor;
        }

        public IImpl Implementor { get; private set; }

        public void Operation()
        {
            // 其他处理
            Implementor.OperationImpl();
        }
    }
}