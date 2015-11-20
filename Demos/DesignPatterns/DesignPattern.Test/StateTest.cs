
using System;
using Common.BehavioralPatterns;
using Common;

namespace DesignPattern.Test
{
    public class StateTest
    {
        public void Test()
        {
            var context = new StateContext<StateTypeA>(new OpenState(), new InputA { Name = "Jacky" });
            context.Request();
            context.Request();
            context.Request();
        }
    }

    public class InputA : IInput
    {
        public string Name { get; set; }
    }

    public class OpenState : IState<StateTypeA>
    {
        public StateTypeA StateType
        {
            get
            {
                return StateTypeA.Open;
            }
        }

        public void Handle(IStateContext<StateTypeA> context)
        {
            var inputA = context.Input as InputA;
            Console.WriteLine("Input Param:" + inputA.Name);
            Console.WriteLine(StateType.ToString() + " is handling context.");

            // change context state
            context.SetState(new QueryState());
        }
    }

    public class QueryState : IState<StateTypeA>
    {
        public StateTypeA StateType
        {
            get
            {
                return StateTypeA.Query;
            }
        }

        public void Handle(IStateContext<StateTypeA> context)
        {
            Console.WriteLine(StateType.ToString() + " is handling context.");

            // change context state
            context.SetState(new CloseState());
        }
    }

    public class CloseState : IState<StateTypeA>
    {
        public StateTypeA StateType
        {
            get
            {
                return StateTypeA.Close;
            }
        }

        public void Handle(IStateContext<StateTypeA> context)
        {
            Console.WriteLine(StateType.ToString() + " is handling context.");
        }
    }


    public enum StateTypeA
    {
        Open,
        Query,
        Close
    }
}
