namespace ZSharp.Framework.BizCode
{
    public class NullSequenceReset : ISequenceReset
    {
        public string Dependency(SequenceContext context)
        {
            return string.Empty;
        }
    }
}
