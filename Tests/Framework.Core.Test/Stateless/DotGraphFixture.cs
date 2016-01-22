using System;
using ZSharp.Framework.Stateless;
using Xunit;

namespace Framework.Stateless.Test
{
    public class DotGraphFixture
    {
        bool IsTrue() 
        {
            return true;
        }

        void OnEntry()
        {

        }

        void OnExit()
        {

        }

        [Fact]
        public void SimpleTransition()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A).Permit(Trigger.X, State.B);
            var graph = sm.ToDotGraph();

            Assert.Equal(expected, graph);
        }

        [Fact]
        public void TwoSimpleTransitions()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X\"];" + System.Environment.NewLine
                         + " A -> C [label=\"Y\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .Permit(Trigger.X, State.B)
                .Permit(Trigger.Y, State.C);

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void WhenDiscriminatedByAnonymousGuard()
        {
            Func<bool> anonymousGuard = () => true;

            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X ["+ anonymousGuard.Method.Name +"]\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .PermitIf(Trigger.X, State.B, anonymousGuard);

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void WhenDiscriminatedByAnonymousGuardWithDescription()
        {
            Func<bool> anonymousGuard = () => true;

            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X [description]\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .PermitIf(Trigger.X, State.B, anonymousGuard, "description");

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void WhenDiscriminatedByNamedDelegate()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X [IsTrue]\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .PermitIf(Trigger.X, State.B, IsTrue);

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void WhenDiscriminatedByNamedDelegateWithDescription()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + " A -> B [label=\"X [description]\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .PermitIf(Trigger.X, State.B, IsTrue, "description");

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void DestinationStateIsDynamic()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + " { node [label=\"?\"] unknownDestination_0 };" + System.Environment.NewLine
                         + " A -> unknownDestination_0 [label=\"X\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);
            sm.Configure(State.A)
                .PermitDynamic(Trigger.X, () => State.B);

            var graph = sm.ToDotGraph();

            Assert.Equal(expected, graph);
        }

        [Fact]
        public void DestinationStateIsCalculatedBasedOnTriggerParameters()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + " { node [label=\"?\"] unknownDestination_0 };" + System.Environment.NewLine
                         + " A -> unknownDestination_0 [label=\"X\"];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);
            var trigger = sm.SetTriggerParameters<int>(Trigger.X);
            sm.Configure(State.A)
                .PermitDynamic(trigger, i => i == 1 ? State.B : State.C);

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void OnEntryWithAnonymousActionAndDescription()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + "node [shape=box];" + System.Environment.NewLine
                         + " A -> \"enteredA\" [label=\"On Entry\" style=dotted];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .OnEntry(() => { }, "enteredA");

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void OnEntryWithNamedDelegateActionAndDescription()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + "node [shape=box];" + System.Environment.NewLine
                         + " A -> \"enteredA\" [label=\"On Entry\" style=dotted];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .OnEntry(OnEntry, "enteredA");

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void OnExitWithAnonymousActionAndDescription()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + "node [shape=box];" + System.Environment.NewLine
                         + " A -> \"exitA\" [label=\"On Exit\" style=dotted];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .OnExit(() => { }, "exitA");

            Assert.Equal(expected, sm.ToDotGraph());
        }

        [Fact]
        public void OnExitWithNamedDelegateActionAndDescription()
        {
            var expected = "digraph {" + System.Environment.NewLine
                         + "node [shape=box];" + System.Environment.NewLine
                         + " A -> \"exitA\" [label=\"On Exit\" style=dotted];" + System.Environment.NewLine
                         + "}";

            var sm = new StateMachine<State, Trigger>(State.A);

            sm.Configure(State.A)
                .OnExit(OnExit, "exitA");

            Assert.Equal(expected, sm.ToDotGraph());
        }
    }
}
