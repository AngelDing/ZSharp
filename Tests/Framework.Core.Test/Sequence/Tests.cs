using System.Threading.Tasks;
using Xunit;
using ZSharp.Framework.Sequence;

namespace Framework.Sequence.Test
{
    public class Tests
    {
        private IStateProvider GetStateProvider()
        {
           return new InMemoryStateProvider();
        }

        private static async Task<ISequence> CreateSequence(IStateProvider stateProvider, int increment = 1, int startAt = 0, long maxValue = long.MaxValue,
            long minValue = long.MinValue,bool cycle = false )
        {
            var options = new SequenceOptions { 
                Increment = increment,
                StartAt = startAt,
                MaxValue = maxValue,
                Cycle = cycle,
                MinValue = minValue
            };
            var sequence = await stateProvider.NewAsync(options);
            return sequence;
        }

        [Fact]
        public async Task NextAsyncReturnsExpectedValue()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider);
            var sequenceKey = await  stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            Assert.True(nextValue1 == 1);
            Assert.True(nextValue2 == 2);
        }

        [Fact]
        public async Task NextAsyncReturnsExpectedValueForNegativeIncrement()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider, increment: -1, startAt: 5, minValue: -100);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            Assert.True(nextValue1 == 4);
            Assert.True(nextValue2 == 3);
        }

        [Fact]
        public async Task NextAsyncReturnsExpectedValueForZeroIncrement()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider,increment: 0, startAt: 5);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            Assert.True(nextValue1 == 5);
            Assert.True(nextValue2 == 5);
        }

        [Fact]
        public async Task NextAsyncReturnsExpectedIncrement()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider,increment: 10);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            Assert.True(nextValue1 == 10);
            Assert.True(nextValue2 == 20);
        }

        [Fact]
        public async Task NextAsyncStartsAtExpectedValue()
        {

            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider, startAt: 20);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            Assert.True(nextValue1 == 21);
            Assert.True(nextValue2 == 22);
        }

        [Fact]
        public async Task NextMethodThrowsExceptionIfSequencyCanNotBeFound()
        {
            var stateProvider = new InMemoryStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            await Assert.ThrowsAsync<SequenceCouldNotBeFoundException>(() => sequenceGenerator.NextAsync(new SequenceKey { Value = "1234" }));
        }

        [Fact]
        public async Task NextMethodThrowsExceptionWhenMaximumValueIsReached()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider, maxValue: 2);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);            
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.True(nextValue1 == 1);
            Assert.True(nextValue2 == 2);
            await Assert.ThrowsAsync<MaximumValueReachedException>(() => sequenceGenerator.NextAsync(sequenceKey));
        }

        [Fact]
        public async Task NextMethodCyclesWhenMaximumValueIsReached()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider, maxValue: 2, cycle: true);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue3 = await sequenceGenerator.NextAsync(sequenceKey);
            Assert.True(nextValue1 == 1);
            Assert.True(nextValue2 == 2);
            Assert.True(nextValue3 == 1);
        }

        [Fact]
        public async Task NextMethodCyclesWhenMinimumValueIsReached()
        {
            var stateProvider = GetStateProvider();
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider, minValue: 2, startAt: 4, increment: -1, cycle: true);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue3 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.True(nextValue1 == 3);
            Assert.True(nextValue2 == 2);
            Assert.True(nextValue3 == 3);
        }

        [Fact]
        public async Task NextMethodThrowsExceptionWhenMinimumValueIsReached()
        {
            var stateProvider = GetStateProvider();

            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider,minValue: 2, startAt: 4, increment: -1, cycle: false);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            var nextValue1 = await sequenceGenerator.NextAsync(sequenceKey);
            var nextValue2 = await sequenceGenerator.NextAsync(sequenceKey);

            Assert.True(nextValue1 == 3);
            Assert.True(nextValue2 == 2);
            await Assert.ThrowsAsync<MinimumValueReachedException>(() => sequenceGenerator.NextAsync(sequenceKey));
        }
       
        [Fact]
        public async Task NextMethodThrowsExceptionWhenIfMaxRetryAttemptIsReach()
        {
            var stateProvider = new InMemoryStateProvider();            
            stateProvider.UpdateValue = false;
            var sequenceGenerator = new SequenceGenerator(stateProvider);
            var sequence = await CreateSequence(stateProvider);
            var sequenceKey = await stateProvider.AddAsync(sequence);
            await Assert.ThrowsAsync<MaxRetryAttemptReachedException>(() => sequenceGenerator.NextAsync(sequenceKey)); 
        }
    }
}
