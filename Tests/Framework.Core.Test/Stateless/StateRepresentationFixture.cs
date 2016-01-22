using System.Collections.Generic;
using ZSharp.Framework.Stateless;
using Xunit;

namespace Framework.Stateless.Test
{
    public class StateRepresentationFixture
    {
        [Fact]
        public void UponEntering_EnteringActionsExecuted()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            StateMachine<State, Trigger>.Transition
                transition = new StateMachine<State, Trigger>.Transition(State.A, State.B, Trigger.X),
                actualTransition = null;
            stateRepresentation.AddEntryAction((t, a) => actualTransition = t, "entryActionDescription");
            stateRepresentation.Enter(transition);
            Assert.Equal(transition, actualTransition);
        }

        [Fact]
        public void UponLeaving_EnteringActionsNotExecuted()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            StateMachine<State, Trigger>.Transition
                transition = new StateMachine<State, Trigger>.Transition(State.A, State.B, Trigger.X),
                actualTransition = null;
            stateRepresentation.AddEntryAction((t, a) => actualTransition = t, "entryActionDescription");
            stateRepresentation.Exit(transition);
            Assert.Null(actualTransition);
        }

        [Fact]
        public void UponLeaving_LeavingActionsExecuted()
        {
            var stateRepresentation = CreateRepresentation(State.A);
            StateMachine<State, Trigger>.Transition
                transition = new StateMachine<State, Trigger>.Transition(State.A, State.B, Trigger.X),
                actualTransition = null;
            stateRepresentation.AddExitAction(t => actualTransition = t, "entryActionDescription");
            stateRepresentation.Exit(transition);
            Assert.Equal(transition, actualTransition);
        }

        [Fact]
        public void UponEntering_LeavingActionsNotExecuted()
        {
            var stateRepresentation = CreateRepresentation(State.A);
            StateMachine<State, Trigger>.Transition
                transition = new StateMachine<State, Trigger>.Transition(State.A, State.B, Trigger.X),
                actualTransition = null;
            stateRepresentation.AddExitAction(t => actualTransition = t, "exitActionDescription");
            stateRepresentation.Enter(transition);
            Assert.Null(actualTransition);
        }

        [Fact]
        public void IncludesUnderlyingState()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            Assert.True(stateRepresentation.Includes(State.B));
        }

        [Fact]
        public void DoesNotIncludeUnrelatedState()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            Assert.False(stateRepresentation.Includes(State.C));
        }

        [Fact]
        public void IncludesSubstate()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            stateRepresentation.AddSubstate(CreateRepresentation(State.C));
            Assert.True(stateRepresentation.Includes(State.C));
        }

        [Fact]
        public void DoesNotIncludeSuperstate()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            stateRepresentation.Superstate = CreateRepresentation(State.C);
            Assert.False(stateRepresentation.Includes(State.C));
        }

        [Fact]
        public void IsIncludedInUnderlyingState()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            Assert.True(stateRepresentation.IsIncludedIn(State.B));
        }

        [Fact]
        public void IsNotIncludedInUnrelatedState()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            Assert.False(stateRepresentation.IsIncludedIn(State.C));
        }

        [Fact]
        public void IsNotIncludedInSubstate()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            stateRepresentation.AddSubstate(CreateRepresentation(State.C));
            Assert.False(stateRepresentation.IsIncludedIn(State.C));
        }

        [Fact]
        public void IsIncludedInSuperstate()
        {
            var stateRepresentation = CreateRepresentation(State.B);
            stateRepresentation.Superstate = CreateRepresentation(State.C);
            Assert.True(stateRepresentation.IsIncludedIn(State.C));
        }

        [Fact]
        public void WhenTransitioningFromSubToSuperstate_SubstateEntryActionsExecuted()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            var executed = false;
            sub.AddEntryAction((t, a) => executed = true, "entryActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(super.UnderlyingState, sub.UnderlyingState, Trigger.X);
            sub.Enter(transition);
            Assert.True(executed);
        }

        [Fact]
        public void WhenTransitioningFromSubToSuperstate_SubstateExitActionsExecuted()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            var executed = false;
            sub.AddExitAction(t => executed = true, "exitActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(sub.UnderlyingState, super.UnderlyingState, Trigger.X);
            sub.Exit(transition);
            Assert.True(executed);
        }

        [Fact]
        public void WhenTransitioningToSuperFromSubstate_SuperEntryActionsNotExecuted()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            var executed = false;
            super.AddEntryAction((t, a) => executed = true, "entryActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(super.UnderlyingState, sub.UnderlyingState, Trigger.X);
            super.Enter(transition);
            Assert.False(executed);
        }

        [Fact]
        public void WhenTransitioningFromSuperToSubstate_SuperExitActionsNotExecuted()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            var executed = false;
            super.AddExitAction(t => executed = true, "exitActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(super.UnderlyingState, sub.UnderlyingState, Trigger.X);
            super.Exit(transition);
            Assert.False(executed);
        }

        [Fact]
        public void WhenEnteringSubstate_SuperEntryActionsExecuted()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            var executed = false;
            super.AddEntryAction((t, a) => executed = true, "entryActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(State.C, sub.UnderlyingState, Trigger.X);
            sub.Enter(transition);
            Assert.True(executed);
        }

        [Fact]
        public void WhenLeavingSubstate_SuperExitActionsExecuted()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            var executed = false;
            super.AddExitAction(t => executed = true, "exitActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(sub.UnderlyingState, State.C, Trigger.X);
            sub.Exit(transition);
            Assert.True(executed);
        }

        [Fact]
        public void EntryActionsExecuteInOrder()
        {
            var actual = new List<int>();

            var rep = CreateRepresentation(State.B);
            rep.AddEntryAction((t, a) => actual.Add(0), "entryActionDescription");
            rep.AddEntryAction((t, a) => actual.Add(1), "entryActionDescription");

            rep.Enter(new StateMachine<State, Trigger>.Transition(State.A, State.B, Trigger.X));

            Assert.Equal(2, actual.Count);
            Assert.Equal(0, actual[0]);
            Assert.Equal(1, actual[1]);
        }

        [Fact]
        public void ExitActionsExecuteInOrder()
        {
            var actual = new List<int>();

            var rep = CreateRepresentation(State.B);
            rep.AddExitAction(t => actual.Add(0), "entryActionDescription");
            rep.AddExitAction(t => actual.Add(1), "entryActionDescription");

            rep.Exit(new StateMachine<State, Trigger>.Transition(State.B, State.C, Trigger.X));

            Assert.Equal(2, actual.Count);
            Assert.Equal(0, actual[0]);
            Assert.Equal(1, actual[1]);
        }

        [Fact]
        public void WhenTransitionExists_TriggerCanBeFired()
        {
            var rep = CreateRepresentation(State.B);
            Assert.False(rep.CanHandle(Trigger.X));
        }

        [Fact]
        public void WhenTransitionDoesNotExist_TriggerCannotBeFired()
        {
            var rep = CreateRepresentation(State.B);
            rep.AddTriggerBehaviour(new StateMachine<State, Trigger>.IgnoredTriggerBehaviour(Trigger.X, () => true));
            Assert.True(rep.CanHandle(Trigger.X));
        }

        [Fact]
        public void WhenTransitionExistsInSupersate_TriggerCanBeFired()
        {
            var rep = CreateRepresentation(State.B);
            rep.AddTriggerBehaviour(new StateMachine<State, Trigger>.IgnoredTriggerBehaviour(Trigger.X, () => true));
            var sub = CreateRepresentation(State.C);
            sub.Superstate = rep;
            rep.AddSubstate(sub);
            Assert.True(sub.CanHandle(Trigger.X));
        }

        [Fact]
        public void WhenEnteringSubstate_SuperstateEntryActionsExecuteBeforeSubstate()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            int order = 0, subOrder = 0, superOrder = 0;
            super.AddEntryAction((t, a) => superOrder = order++, "entryActionDescription");
            sub.AddEntryAction((t, a) => subOrder = order++, "entryActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(State.C, sub.UnderlyingState, Trigger.X);
            sub.Enter(transition);
            Assert.True(superOrder < subOrder);
        }

        [Fact]
        public void WhenExitingSubstate_SubstateEntryActionsExecuteBeforeSuperstate()
        {
            StateMachine<State, Trigger>.StateRepresentation super;
            StateMachine<State, Trigger>.StateRepresentation sub;
            CreateSuperSubstatePair(out super, out sub);

            int order = 0, subOrder = 0, superOrder = 0;
            super.AddExitAction(t => superOrder = order++, "entryActionDescription");
            sub.AddExitAction(t => subOrder = order++, "entryActionDescription");
            var transition = new StateMachine<State, Trigger>.Transition(sub.UnderlyingState, State.C, Trigger.X);
            sub.Exit(transition);
            Assert.True(subOrder < superOrder);
        }

        void CreateSuperSubstatePair(out StateMachine<State, Trigger>.StateRepresentation super, out StateMachine<State, Trigger>.StateRepresentation sub)
        {
            super = CreateRepresentation(State.A);
            sub = CreateRepresentation(State.B);
            super.AddSubstate(sub);
            sub.Superstate = super;
        }

        StateMachine<State, Trigger>.StateRepresentation CreateRepresentation(State state)
        {
            return new StateMachine<State, Trigger>.StateRepresentation(state);
        }
    }
}
