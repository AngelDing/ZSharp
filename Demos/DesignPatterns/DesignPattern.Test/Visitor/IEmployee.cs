using Common.BehavioralPatterns;

namespace DesignPattern.Test.Visitor
{
    /// <summary>
    /// Visitor需要影响的Element
    /// </summary>
    public interface IEmployee : IVisitorElement
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        decimal Income { get; set; }

        /// <summary>
        /// 年假天数
        /// </summary>
        int VacationDays { get; set; }

        /// <summary>
        /// 薪酬支付类型
        /// </summary>
        PayType PayType { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        JobLevel JobLevel { get; set; }

        string PayReport { get; set; }
    }

    public enum PayType
    {
        Hourly,

        Weekly,

        Monthly,

        Yearly
    }

    public enum JobLevel
    {
        Common,
        Manager,
        SeniorManager
    }
}
