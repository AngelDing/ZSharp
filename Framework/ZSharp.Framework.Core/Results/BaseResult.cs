using System;

namespace ZSharp.Framework.Results
{
    /// <summary>
    /// 表示一个结果。
    /// </summary>
    public class BaseResult : IResult
    {
        private string message = string.Empty;
        private Exception exception;
        private ResultStatusType status = ResultStatusType.Success;
        internal const string SuccessedString = "执行成功！";

        /// <summary>
        /// 表示成功、且无法修改的结果。
        /// </summary>
        public readonly static BaseResult Successfully = new SuccessfullyResult();        

        /// <summary>
        /// 获取或设置执行结果描述错误的信息。
        /// </summary>
        public virtual string Message { get { return this.message; } set { this.ToFailded(value); } }

        /// <summary>
        /// 获取或设置执行时发生的错误。结果状态ResultStatusType.Success时，该值为 null 值。
        /// </summary>
        public virtual Exception Exception
        {
            get
            {
                if (this.exception == null && this.IsFailed)
                {
                    this.exception = new ResultException(this.message, this.status);
                }
                return this.exception;
            }
            set 
            { 
                this.ToFailded(value);
            }
        }

        /// <summary>
        /// 获取一个值，表示执行结果是否为失败。
        /// </summary>
        public virtual bool IsFailed { get { return this.status == ResultStatusType.Faliure; } }

        /// <summary>
        /// 获取一个值，表示执行结果是否为成功。
        /// </summary>
        public virtual bool IsSucceed { get { return this.status == ResultStatusType.Success; } }

        /// <summary>
        /// 获取执行的状态码。
        /// </summary>
        public virtual ResultStatusType Status { get { return this.status; } set { this.status = value; } }

        public BaseResult()
        { 
        }

        /// <summary>
        /// 指定引发的异常和状态码，初始化一个 BaseResult 类的新实例。
        /// </summary>
        /// <param name="exception">引发的异常。如果为 null 值，将不会更改返回结果的状态。</param>
        /// <param name="status">结果的状态码。</param>
        public BaseResult(Exception exception, ResultStatusType status = ResultStatusType.Faliure)
        {
            this.ToFailded(exception, status);
        }

        /// <summary>
        /// 指定描述错误的信息和状态码，初始化一个 BaseResult 类的新实例。
        /// </summary>
        /// <param name="mssage">描述错误的信息。如果为 null 值，将不会更改返回结果的状态。</param>
        /// <param name="status">结果的状态码。</param>
        public BaseResult(string mssage, ResultStatusType status = ResultStatusType.Faliure)
        {
            this.ToFailded(mssage, status);
        }

        /// <summary>
        /// 返回以字符串形式描述的结果。
        /// </summary>
        /// <returns>如果这是一个成功的操作结果，将返回“执行成功！”，否则返回异常的描述信息。</returns>
        public override string ToString()
        {
            if (this.IsSucceed)
            {
                return BaseResult.SuccessedString;
            }
            return this.message;
        }
    }
}
