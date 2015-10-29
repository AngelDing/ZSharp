using System;

namespace ZSharp.Framework.MongoDb.Managers
{
    public class ProfileModel
    {
        public string Client { get; set; }

        public string Op { get; set; }

        public string Namespace { get; set; }

        public string Command { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public double Duration { get; set; }

        public int NumberToReturn { get; set; }

        public int NumberScanned { get; set; }

        public int NumberReturned { get; set; }

        public int ResponseLength { get; set; }
    }
}
