using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using ZSharp.Framework;
using ZSharp.Framework.Domain;

namespace Demos.CQRS.Subscriber
{
    public class CqrsDemoProcessor : DisposableObject
    {
        private IUnityContainer container;
        private CancellationTokenSource cancellationTokenSource;
        private List<IProcessor> processors;

        public CqrsDemoProcessor()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.container = CreateContainer();
            this.processors = this.container.ResolveAll<IProcessor>().ToList();
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

        private IUnityContainer CreateContainer()
        {
            var container = UnityConfig.GetConfiguredContainer();
            return container;          
        }
    }
}
