using Common.BehavioralPatterns;
using System;

namespace DesignPattern.Test.Visitor
{
    public interface IEmployeeVisitor : IVisitor
    {
    }

    public class HourlyEmployeeVisitor : IEmployeeVisitor
    {
        public void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            if (employee.PayType == PayType.Hourly)
            {
                employee.PayReport = "100 Hours and $1000 in total.";
            }
        }
    }

    public class MonthlyEmployeeVisitor : IEmployeeVisitor
    {
        public void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            if (employee.PayType == PayType.Monthly)
            {
                employee.PayReport = "Monthly pay : $3000.";
            }
        }
    }

    public class YearlyEmployeeVisitor : IEmployeeVisitor
    {
        public void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            if (employee.PayType == PayType.Yearly)
            {
                employee.PayReport = "Yearly pay : $200000.";
            }
        }
    }

    public class CommonEmployeeVisitor : IEmployeeVisitor
    {
        protected decimal defaultIncome = 3000;
        protected int defaultVacationDays = 5;

        public virtual void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            if (employee.JobLevel == JobLevel.Common)
            {
                employee.Income = defaultIncome;
                employee.VacationDays = defaultVacationDays;
            }
        }
    }

    public class ManagerEmployeeVisitor : CommonEmployeeVisitor
    {
        public override void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            if (employee.JobLevel == JobLevel.Manager)
            {
                employee.Income = defaultIncome * 1.2M;
                employee.VacationDays = defaultVacationDays + 3;
            }
        }
    }

    public class SeniorManagerEmployeeVisitor : CommonEmployeeVisitor
    {
        public override void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            if (employee.JobLevel == JobLevel.SeniorManager)
            {
                employee.Income = defaultIncome * 1.5M;
                employee.VacationDays = defaultVacationDays + 5;
            }
        }
    }
}
