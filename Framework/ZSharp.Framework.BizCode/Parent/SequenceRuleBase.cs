using System;

namespace ZSharp.Framework.BizCode
{
    public abstract class SequenceRuleBase
    {
        /// <summary>
        /// 补齐宽度
        /// </summary>
        public int PaddingWidth { get; set; }
        /// <summary>
        /// 补齐字符
        /// </summary>
        public char PaddingChar { get; set; }
        /// <summary>
        /// 补齐方向
        /// </summary>
        public PaddingSide PaddingSide { get; set; }

        /// <summary>
        /// 规则参数 如"yyyy-mm-dd"
        /// </summary>
        public string RuleValue { get; set; }

        public SequenceRuleBase()
        {
            PaddingWidth = 0;
            PaddingChar = char.MinValue;
            PaddingSide = PaddingSide.None;
            RuleValue = "";
        }

        public string Series(SequenceContext data)
        {
            var result = Handle(data);
            result = result ?? string.Empty;
            if (PaddingSide == PaddingSide.Left && PaddingWidth > 0)
            {
                if (PaddingChar == char.MinValue)
                    throw new Exception(string.Format("取得Sequence[{0}]处理中未设置填充的字符", data.SequenceName));

                result = result.PadLeft(PaddingWidth, PaddingChar);
            }
            else if (PaddingSide == PaddingSide.Right && PaddingWidth > 0)
            {
                if (PaddingChar == char.MinValue)
                    throw new Exception(string.Format("取得Sequence[{0}]处理中未设置填充的字符", data.SequenceName));

                result = result.PadRight(PaddingWidth, PaddingChar);
            }

            return result;
        }
        /// <summary>
        /// 具体的规则处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract string Handle(SequenceContext data);
    }
}
