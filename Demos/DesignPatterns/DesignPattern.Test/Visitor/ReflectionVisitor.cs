using System;
using Common.BehavioralPatterns;
using System.Reflection;
using System.Collections.Concurrent;

namespace DesignPattern.Test.Visitor
{
    public abstract class BaseVisitor
    {
        private ConcurrentDictionary<string, MethodInfo> methodCache =
           new ConcurrentDictionary<string, MethodInfo>();

        protected void GetMethodInfo(string methodName, object paramObj)
        {
            MethodInfo methodInfo;
            var type = this.GetType();
            var key = type.Name + methodName;
            if (!methodCache.TryGetValue(key, out methodInfo))
            {
                methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                methodCache.TryAdd(key, methodInfo);
            }
            methodInfo.Invoke(this, new object[] { paramObj });
        }
    }

    public interface IEmployeeReflectionVisitor : IVisitor
    {
    }

    public class EmployeeVisitor : BaseVisitor, IEmployeeReflectionVisitor
    {
        private const string visitMethod = "Visit{0}";       

        public void Visit(IVisitorElement element)
        {
            var employee = element as IEmployee;
            var payName = employee.PayType.ToString() + "Pay";
            var methodName = string.Format(visitMethod, payName);

            //var thisType = this.GetType();
            GetMethodInfo(methodName, employee);
            //var method = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            //method.Invoke(this, new object[] { employee });

            var levelName = employee.JobLevel.ToString() + "Level";
            methodName = string.Format(visitMethod, levelName);
            GetMethodInfo(methodName, employee);
            //method = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            //method.Invoke(this, new object[] { employee });
        }       

        private void VisitMonthlyPay(IEmployee employee)
        {
            if (employee.PayType == PayType.Monthly)
            {
                employee.PayReport = "Monthly pay : $3000.";
            }
        }

        private void VisitYearlyPay(IEmployee employee)
        {
            if (employee.PayType == PayType.Yearly)
            {
                employee.PayReport = "Yearly pay : $200000.";
            }
        }

        private decimal defaultIncome = 3000;
        private int defaultVacationDays = 5;

        private void VisitCommonLevel(IEmployee employee)
        {
            if (employee.JobLevel == JobLevel.Common)
            {
                employee.Income = defaultIncome;
                employee.VacationDays = defaultVacationDays;
            }
        }

        private void VisitManagerLevel(IEmployee employee)
        {
            if (employee.JobLevel == JobLevel.Manager)
            {
                employee.Income = defaultIncome * 1.2M;
                employee.VacationDays = defaultVacationDays + 3;
            }
        }

        private void VisitSeniorManagerLevel(IEmployee employee)
        {
            if (employee.JobLevel == JobLevel.SeniorManager)
            {
                employee.Income = defaultIncome * 1.5M;
                employee.VacationDays = defaultVacationDays + 5;
            }
        }     
    }
}
