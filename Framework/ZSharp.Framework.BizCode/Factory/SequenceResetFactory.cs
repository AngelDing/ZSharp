using System;
using System.Linq;
using System.Reflection;

namespace ZSharp.Framework.BizCode
{
    public class SequenceResetFactory
    {
        public static ISequenceReset CreateReset(string sequenceReset)
        {
            if (string.IsNullOrEmpty(sequenceReset))
                return new NullSequenceReset();

            var type = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterface("ISequenceReset") != null && t.Name.Equals(sequenceReset + "SequenceReset", StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (type == null)
                throw new Exception(string.Format("无法创建重置依赖[{0}],找不到类{0}SequenceReset", sequenceReset));

            return (ISequenceReset)Activator.CreateInstance(type);
        }
    }
}
