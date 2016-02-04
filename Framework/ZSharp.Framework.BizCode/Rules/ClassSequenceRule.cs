using System;

namespace ZSharp.Framework.BizCode
{
    public class ClassSequenceRule : SequenceRuleBase
    {
        private IClassSequenceHandler handler;

        protected override string Handle(SequenceContext data)
        {
            if (handler == null)
            {
                var type = Type.GetType(RuleValue);
                if (type == null)
                    throw new Exception(string.Format("取得Sequence[{0}]函数处理规则中类名设置不正确", data.SequenceName));

                if (type.GetInterface("IClassSequenceHandler") == null)
                    throw new Exception(string.Format("取得Sequence[{0}]函数处理{0}未实现接口IClassSequenceHandler", type.Name));

                handler = (IClassSequenceHandler)Activator.CreateInstance(type);
            }

            return handler.Handle(data);
        }
    }
}
