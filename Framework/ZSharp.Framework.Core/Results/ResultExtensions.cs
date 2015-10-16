using System;

namespace ZSharp.Framework.Results
{
    public static class ResultExtensions
    {
        public static TResult ToFailded<TResult>(this TResult result, Exception exception, ResultStatusType status = ResultStatusType.Faliure)
            where TResult : BaseResult
        {
            if (result == null) throw new ArgumentNullException("result");

            if (exception != null)
            {
                result.Message = exception.Message;
                result.Exception = exception;
                result.Status = status;
            }
            return result;
        }

        public static TResult ToFailded<TResult>(this TResult result, string message, ResultStatusType status = ResultStatusType.Faliure)
            where TResult : BaseResult
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (message != null)
            {
                result.Message = message;
                result.Status = status;
            }
            return result;
        }

        public static TResult ToSuccessed<TResult>(this TResult result)
           where TResult : BaseResult
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            result.Status = ResultStatusType.Success;
            result.Message = null;
            result.Exception = null;
            return result;
        }

        public static TResult ToSuccessed<TResult, TValue>(this TResult result, TValue value)
            where TResult : BaseResult<TValue>
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            result.Value = value;
            return ToSuccessed(result);
        }
    }
}
