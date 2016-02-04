using System;

namespace ZSharp.Framework.BizCode
{
    public class GuidSequenceRule : SequenceRuleBase
    {
        protected override string Handle(SequenceContext data)
        {
            return Guid.NewGuid().ToString(RuleValue);
        }
    }
}
