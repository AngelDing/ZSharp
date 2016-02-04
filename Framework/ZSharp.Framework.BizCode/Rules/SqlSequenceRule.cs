namespace ZSharp.Framework.BizCode
{
    public class SQLSequenceRule : SequenceRuleBase
    {
        protected override string Handle(SequenceContext data)
        {
            //return data.db.Sql(RuleValue).QuerySingle<string>();
            return "";
        }
    }
}
