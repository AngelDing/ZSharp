namespace ZSharp.Framework.BizCode
{
    public class ConstSequenceRule : SequenceRuleBase
    {
        protected override string Handle(SequenceContext data)
        {
            return RuleValue ?? string.Empty;
        }
    }
}
