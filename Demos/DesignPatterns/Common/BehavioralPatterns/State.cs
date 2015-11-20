
using System;

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
