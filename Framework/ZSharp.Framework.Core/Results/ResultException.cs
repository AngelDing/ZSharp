using System;

namespace ZSharp.Framework.Results
{
    public class ResultException : Exception
    {
        public ResultException(string message, ResultStatusType status = ResultStatusType.Faliure)
            : base(message)
        {
            if (status == ResultStatusType.Success)
            {
                throw new ArgumentOutOfRangeException("status error");
            }
        }
    }
}
