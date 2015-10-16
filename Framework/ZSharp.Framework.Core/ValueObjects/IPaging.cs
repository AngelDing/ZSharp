
namespace ZSharp.Framework.ValueObjects
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPaging
    {
        /// <summary>
        /// 满足查询条件的总记录数
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }
    }
}
