using System;
using ZSharp.Framework.MongoDb;

namespace Framework.MongoDb.Test
{
    public class MongoTaskEntity : StringKeyMongoEntity
    {
        public string Name { get; set; }

        public TaskStatusEntity TaskStatusEntity { get; set; }

        public TaskEntity TaskEntity { get; set; }
    }

    public class MongoTaskEntity2 : MongoTaskEntity
    {
    }

    public class MongoTaskEntity3 : MongoTaskEntity
    {
    }

    public class TaskStatusEntity
    {
        public DateTime LastRunTime { get; set; }
    }

    public class TaskEntity
    {
        public TimeSpan StartTime { get; set; }
    }
}
