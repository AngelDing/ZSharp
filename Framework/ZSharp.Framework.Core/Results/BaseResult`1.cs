using System;

namespace ZSharp.Framework.Results
{
    public class BaseResult<TValue> : BaseResult, IResult<TValue>
    {
        protected internal TValue value;
        public virtual TValue Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.ToSuccessed(value);
            }
        }

        public BaseResult()
        { 
        }

        public BaseResult(TValue value)
        {
            this.value = value;
        }

        public BaseResult(Exception exception, ResultStatusType status = ResultStatusType.Faliure)
            : base(exception, status)
        {
        }

        public BaseResult(string message, ResultStatusType status = ResultStatusType.Faliure)
            : base(message, status)
        {
        }

        public override string ToString()
        {
            if (this.IsSucceed)
            {
                if (this.value == null)
                {
                    return null;
                }
                return this.value.ToString();
            }
            return base.ToString();
        }
    }
}
