using System.Collections.Generic;

namespace ZSharp.Framework.BizCode
{
    public class SequenceContext
    {
        /// <summary>
        /// 数据库访问
        /// </summary>
        public ISequenceRepository Repo { get; set; }

        /// <summary>
        /// 重置依赖规则
        /// </summary>
        public ISequenceReset SequenceReset { get; set; }

        /// <summary>
        /// 编号规则
        /// </summary>
        public List<SequenceRuleBase> Rules { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public string TenantID { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// 分割符号
        /// </summary>
        public string SequenceDelimiter { get; set; }

        /// <summary>
        /// 步伐
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 当前序号
        /// </summary>
        public int CurrentNo { get; set; }

        /// <summary>
        /// 当前编号
        /// </summary>
        public string CurrentCode { get; set; }

        /// <summary>
        /// 当前重置依赖
        /// </summary>
        public string CurrentReset { get; set; }

        /// <summary>
        /// 是否多依赖
        /// </summary>
        public bool IsMultipleTenant { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> row { get; set; }

        public SequenceContext()
        {
            Repo = null;
            SequenceReset = new NullSequenceReset();
            Rules = new List<SequenceRuleBase>();
            TenantID = "";
            SequenceName = "";
            SequenceDelimiter = "";
            Step = 0;
            CurrentNo = 0;
            CurrentCode = "";
            IsMultipleTenant = true;
            row = new Dictionary<string, object>();
        }
    }
}
