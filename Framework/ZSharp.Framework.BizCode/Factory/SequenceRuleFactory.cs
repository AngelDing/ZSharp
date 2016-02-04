using System;
using System.Linq;
using System.Reflection;

namespace ZSharp.Framework.BizCode
{
    public class SequenceRuleFactory
    {
        public static SequenceRuleBase CreateRule(string ruleName)
        {
            var type = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType == typeof(SequenceRuleBase) && t.Name.Equals(ruleName + "SequenceRule", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (type == null)
                throw new Exception(string.Format("无法创建编码规则[{0}],找不到类{0}SequenceRule", ruleName));

            return (SequenceRuleBase)Activator.CreateInstance(type);
        }

        internal static object CreateRule(object ruleName)
        {
            throw new NotImplementedException();
        }
    }
}
