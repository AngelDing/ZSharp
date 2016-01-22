using System;
using ZSharp.Framework.Stateless;
using Xunit;

namespace Framework.Stateless.Test
{
    public class TriggerWithParametersFixture
    {
        [Fact]
        public void DescribesUnderlyingTrigger()
        {
            var twp = new StateMachine<State, Trigger>.TriggerWithParameters<string>(Trigger.X);
            Assert.Equal(Trigger.X, twp.Trigger);
        }

        [Fact]
        public void ParametersOfCorrectTypeAreAccepted()
        {
            var twp = new StateMachine<State, Trigger>.TriggerWithParameters<string>(Trigger.X);
            twp.ValidateParameters(new [] { "arg" });
        }

        [Fact]
        public void ParametersArePolymorphic()
        {
            var twp = new StateMachine<State, Trigger>.TriggerWithParameters<object>(Trigger.X);
            twp.ValidateParameters(new[] { "arg" });
        }

        public void IncompatibleParametersAreNotValid()
        {
            var twp = new StateMachine<State, Trigger>.TriggerWithParameters<string>(Trigger.X);
            Assert.Throws<ArgumentException>(delegate { twp.ValidateParameters(new object[] { 123 }); });
        }

        public void TooFewParametersDetected()
        {
            var twp = new StateMachine<State, Trigger>.TriggerWithParameters<string, string>(Trigger.X);
            Assert.Throws<ArgumentException>(delegate { twp.ValidateParameters(new[] { "a" }); });
            
        }

        public void TooManyParametersDetected()
        {
            var twp = new StateMachine<State, Trigger>.TriggerWithParameters<string, string>(Trigger.X);            
            Assert.Throws<ArgumentException>(delegate { twp.ValidateParameters(new[] { "a", "b", "c" }); });
        }
    }
}
