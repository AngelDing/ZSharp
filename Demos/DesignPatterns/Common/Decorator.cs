/// <summary>
/// 装饰器模式：动态地给一个对象添加一些额外的职责。就增加功能来说，Decorator 模式相比生成子类更为灵活。
/// 看起来很像 Adapter，但没有 Covert Interface，接口还是原来的接口，只是在具体内部实现中增加了新的职责。
/// 也可采用打Attribute的形式，对类方法采用AOP来装饰某个对象操作；
/// </summary>
namespace Common
{
    public interface IComponent
    {
        void Operation();
    }

    public interface IDecorator : IComponent
    {
    }

    public class ConcreteComponent : IComponent
    {
        public void Operation()
        {
            // do something
        }
    }

    public abstract class DecoratorBase : IDecorator    // is a
    {
        /// <summary>
        /// has a 
        /// </summary>
        protected IComponent target;

        public DecoratorBase(IComponent target)
        {
            this.target = target;
        }

        public virtual void Operation()
        {
            target.Operation();
        }
    }

    public class ConcreteDecorator : DecoratorBase
    {
        public ConcreteDecorator(IComponent component)
            : base(component)
        {
        }

        public override void Operation()
        {
            base.Operation();
            AddedBehavior();
        }

        private void AddedBehavior()
        {
            // do some other things
        }
    }

    public class DecoratorClient
    {
        public void TestCase1()
        {
            var component1 = new ConcreteComponent();
            var component2 = new ConcreteDecorator(component1);
            component2.Operation();
        }
    }
}
