using ZSharp.Framework;
using ZSharp.Framework.Domain;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZSharp.Domain.Demo
{
    public class InventoryProcessor : DisposableObject
    {
        private IUnityContainer container;
        private CancellationTokenSource cancellationTokenSource;
        private List<IProcessor> processors;

        public InventoryProcessor()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.container = CreateContainer();

            this.processors = this.container.ResolveAll<IProcessor>().ToList();
        }

        private IUnityContainer CreateContainer()
        {
            var container = UnityConfig.GetConfiguredContainer();
            return container;
        }

        public void Start()
        {
            this.processors.ForEach(p => p.Start());
        }

        public void Stop()
        {
            this.cancellationTokenSource.Cancel();

            this.processors.ForEach(p => p.Stop());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.container.Dispose();
                this.cancellationTokenSource.Dispose();
            }
        }
    }
}
