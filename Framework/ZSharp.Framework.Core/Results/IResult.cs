using System;

namespace ZSharp.Framework.Results
{
    /// <summary>
    /// 定义一个结果。
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// 获取或设置执行结果描述或错误信息。
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 获取或设置执行时发生的错误。
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// 获取一个值，表示执行结果是否为失败。
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// 获取一个值，表示执行结果是否为成功。
        /// </summary>
        bool IsSucceed { get; }

        /// <summary>
        /// 获取执行结果的状态。
        /// </summary>
        ResultStatusType Status { get; set; }
    }

    /// <summary>
    /// 定义包含一个返回值的结果。
    /// </summary>
    /// <typeparam name="TValue">返回值的数据类型。</typeparam>
    public interface IResult<TValue> : IResult
    {
        /// <summary>
        /// 获取或设置结果的返回值。
        /// </summary>
        TValue Value { get; set; }
    }
}
