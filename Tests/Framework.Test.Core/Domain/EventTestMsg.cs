
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

        public string CreatedBy
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTimeOffset CreationTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public Guid SourceId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
