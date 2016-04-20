using Xunit;
using ZSharp.Framework;

namespace Framework.Core.Test
{
    public class AsyncSemaphoreTest
    {
        [Fact]
        public void TestWaitRelease()
        {
            var semaphore = new AsyncSemaphore(1);
            Assert.Equal(1, semaphore.Available);
            semaphore.Wait();
            Assert.Equal(0, semaphore.Available);
            semaphore.Release();
            Assert.Equal(1, semaphore.Available);
        }
    }
}
