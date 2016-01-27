
namespace ZSharp.Framework.Domain
{
    public class Constants
    {
        public static class ApplicationRuntime
        {
            /// <summary>
            /// Represents the default version number.
            /// </summary>
            public static readonly int DefaultVersion = -1;

            public static readonly string DefaultEventTopic = "CommonEventTopic";

            public static readonly string DefaultCommandTopic = "CommonCommandTopic";

            /// <summary>
            /// 默认快照间隔事件数量
            /// </summary>
            public static readonly int DefaultSnapshotIntervalInEvents = 5;

            public static readonly string DefaultUserName = "system";
        }
    }
}
