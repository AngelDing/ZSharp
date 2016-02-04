using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.BizCode
{
    public class SequenceSettingEntity : Entity<Guid>
    {
        public object PaddingChar { get; internal set; }

        public string PaddingSide { get; internal set; }

        public int PaddingWidth { get; internal set; }

        public string RuleName { get; internal set; }

        public string RuleValue { get; internal set; }
    }
}
