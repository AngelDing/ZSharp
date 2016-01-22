using ZSharp.Framework.Stateless;
using Xunit;

namespace Framework.Stateless.Test
{
    public class TransitioningTriggerBehaviourFixture
    {
        [Fact]
        public void TransitionsToDestinationState()
        {
            var transtioning = new StateMachine<State, Trigger>.TransitioningTriggerBehaviour(Trigger.X, State.C, () => true);
            State destination;
            Assert.True(transtioning.ResultsInTransitionFrom(State.B, new object[0], out destination));
            Assert.Equal(State.C, destination);
        }
    }
}
