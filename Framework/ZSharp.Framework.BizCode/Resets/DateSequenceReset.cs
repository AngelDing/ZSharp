using System;

namespace ZSharp.Framework.BizCode
{
    public class DateSequenceReset : ISequenceReset
    {
        public string Dependency(SequenceContext context)
        {
            var result = DateTime.Now.ToString("yyyyMMdd HH:mm");
            if (result != context.CurrentReset)
            {
                //重置规则
                context.CurrentNo = 0;
            }
            context.CurrentReset = result;
            return result;
        }
    }
}
