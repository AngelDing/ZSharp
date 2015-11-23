using Common.BehavioralPatterns;
using System.Collections.Generic;

namespace DesignPattern.Test.Visitor
{
    public class Employee : IEmployee
    {
        public Employee(string name, JobLevel level, PayType payType)
        {
            this.Name = name;
            this.JobLevel = level;
            this.PayType = payType;
        }

        public decimal Income { get; set; }

        public JobLevel JobLevel { get; set; }

        public string Name { get; set; }

        public PayType PayType { get; set; }

        public int VacationDays { get; set; }

        public string PayReport { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// 另一个具体的Element
    /// </summary>
    public class Manager : Employee
    {
        public string Department { get; set; }

        public Manager(string name, JobLevel level, PayType payType, string department)
            : base(name, level, payType)
        {
            this.Department = department;
        }
    }

    /// <summary>
    /// 为了便于对HR系统的对象进行批量处理增加的集合类型
    /// </summary>
    public class EmployeeCollection : List<IEmployee>
    {
        /// <summary>
        /// 组合起来的批量Accept操作
        /// </summary>
        /// <param name="visitor"></param>
        public virtual void Accept(IVisitor visitor)
        {
            foreach (IEmployee employee in this)
            {
                employee.Accept(visitor);
            }
        }
    }
}
