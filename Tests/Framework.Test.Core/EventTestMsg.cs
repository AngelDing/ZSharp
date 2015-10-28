
using System;
using ZSharp.Framework.Domain;
using ZSharp.Framework.Utils;

namespace Framework.Test.Core
{
    public class EventTestMsg : IEvent
    {
        public EventTestMsg()
        {
            this.Id = GuidHelper.NewSequentialId();
        }
        public Guid Id { get; private set; }

        public string Name { get; set; }
    }
}
