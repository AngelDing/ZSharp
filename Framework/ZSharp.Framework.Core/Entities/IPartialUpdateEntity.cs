using System.Collections.Generic;

namespace ZSharp.Framework.Entities
{
    /// <summary>
    /// 局部更新接口
    /// </summary>
    public interface IPartialUpdateEntity
    {
        /// <summary>
        /// 需要更新的字段及其值的集合
        /// </summary>
        Dictionary<string, object> NeedUpdateList { get; }
    }
}
