/// <summary>
/// 状态模式，又称状态对象模式（Pattern of Objects for States），状态模式是对象的行为模式。
/// 状态模式允许一个对象在其内部状态改变的时候改变其行为。这个对象看上去就像是改变了它的类一样。
/// 所谓对象的状态，通常指的就是对象实例的属性的值；而行为指的就是对象的功能，再具体点说，行为大多可以对应到方法上。
/// 状态模式的功能就是分离状态的行为，通过维护状态的变化，来调用不同状态对应的不同功能。
/// 也就是说，状态和行为是相关联的，它们的关系可以描述为：状态决定行为。
/// 由于状态是在运行期被改变的，因此行为也会在运行期根据状态的改变而改变。
/// </summary>

namespace Common.BehavioralPatterns
{
    public interface IState<T>
    {
        T StateType { get; }

        void Handle(IStateContext<T> context);
    }

    public interface IStateContext<T>
    {
        IInput Input { get; }

        void Request();

        void SetState(IState<T> state);
    }

    public class StateContext<T> : IStateContext<T>
    {
        private IState<T> state;

        public IInput Input{ get; private set; }

        public StateContext(IState<T> state)
            : this(state, null)
        {
        }

        public StateContext(IState<T> state, IInput input)
        {
            this.state = state;
            this.Input = input;
        }

        public virtual void Request()
        {
            this.state.Handle(this);
        }

        public virtual void SetState(IState<T> state)
        {
            this.state = state;
        }
    }
}
