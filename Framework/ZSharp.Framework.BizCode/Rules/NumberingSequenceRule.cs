namespace ZSharp.Framework.BizCode
{
    public class NumberingSequenceRule : SequenceRuleBase
    {
        protected override string Handle(SequenceContext data)
        {
            data.CurrentNo = data.CurrentNo + data.Step;
            return data.CurrentNo.ToString();
        }
    }
}
