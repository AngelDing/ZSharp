using System;
using System.Security.Cryptography;

namespace ZSharp.Framework.MongoDb.Managers
{
    public static class ConstHelper
    {
        public static readonly string ConnString = "mongodb://{0}/{1}";

        public static readonly string AdminDBName = "admin";

        public static readonly string IndexTableName = "system.indexes";

        public static readonly string ProfileTableName = "system.profile";

        public static string OplogTableName = "oplog.rs";

        public static string SourceTableName = "sources";

        public static string LocalDBName = "local";

        private static readonly int StepLength = 8;
        public static int GetRandomId()
        {
            var bytes = new byte[StepLength];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
