using System;

namespace ZSharp.Framework.BizCode
{
    public class TimeStampSequenceRule : SequenceRuleBase
    {
        protected override string Handle(SequenceContext data)
        {
               var result =  DateTime.Now.ToString(RuleValue);
            return result;
        }
    }
}
