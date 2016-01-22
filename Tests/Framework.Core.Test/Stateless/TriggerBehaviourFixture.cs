using ZSharp.Framework.Stateless;
using Xunit;

namespace Framework.Stateless.Test
{
    public class TriggerBehaviourFixture
    {
        [Fact]
        public void ExposesCorrectUnderlyingTrigger()
        {
            var transtioning = new StateMachine<State, Trigger>.TransitioningTriggerBehaviour(
                Trigger.X, State.C, () => true);

            Assert.Equal(Trigger.X, transtioning.Trigger);
        }

        [Fact]
        public void WhenGuardConditionFalse_IsGuardConditionMetIsFalse()
        {
            var transtioning = new StateMachine<State, Trigger>.TransitioningTriggerBehaviour(
                Trigger.X, State.C, () => false);

            Assert.False(transtioning.IsGuardConditionMet);
        }

        [Fact]
        public void WhenGuardConditionTrue_IsGuardConditionMetIsTrue()
        {
            var transtioning = new StateMachine<State, Trigger>.TransitioningTriggerBehaviour(
                Trigger.X, State.C, () => true);

            Assert.True(transtioning.IsGuardConditionMet);
        }
    }
}
