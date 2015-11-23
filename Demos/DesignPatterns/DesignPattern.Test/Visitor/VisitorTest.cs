using System;
using System.Linq;
using System.Collections.Generic;
using Common.BehavioralPatterns;

namespace DesignPattern.Test.Visitor
{
    public class VisitorTest
    {
        private EmployeeCollection employees = new EmployeeCollection();

        public void Test()
        {
            employees.Add(new Employee("joe", JobLevel.Common, PayType.Monthly));
            employees.Add(new Manager("alice", JobLevel.Manager, PayType.Yearly, "sales"));
            employees.Add(new Manager("alice", JobLevel.SeniorManager, PayType.Yearly, "it"));
            employees.Add(new Manager("jacky", JobLevel.SeniorManager, PayType.Yearly, "manager"));

            //AcceptReflectionVisitor(employees);
            AcceptVisitor(employees);

            IEmployee joe = employees[0];
            Console.WriteLine(joe.Income);
            Console.WriteLine(joe.VacationDays);
            Console.WriteLine(joe.PayReport);

            IEmployee alice = employees[1];
            Console.WriteLine(alice.Income);
            Console.WriteLine(alice.VacationDays);
            Console.WriteLine(alice.PayReport);

            IEmployee peter = employees[2];
            Console.WriteLine(peter.Income);
            Console.WriteLine(peter.VacationDays);
            Console.WriteLine(peter.PayReport);

            IEmployee jacky = employees[3];
            Console.WriteLine(jacky.Income);
            Console.WriteLine(jacky.VacationDays);
            Console.WriteLine(jacky.PayReport);

        }

        public void AcceptReflectionVisitor(EmployeeCollection employees)
        {
            employees.Accept(new EmployeeVisitor());
        }
        
        public void AcceptVisitor(EmployeeCollection employees)
        {
            //TODO: 重构：可以放入缓存，IEmployeeVisitor可以按参数传入，提取公共方法
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var visitorList = new List<Type>();
            foreach (var a in assemblies)
            {
                var tempList = a.GetTypes();
                foreach (var t in tempList)
                {
                    if (typeof(IEmployeeVisitor).IsAssignableFrom(t) && !t.IsInterface)
                    {
                        visitorList.Add(t);
                    }
                }
            }

            foreach (var v in visitorList)
            {
                var instance = Activator.CreateInstance(v);
                employees.Accept(instance as IVisitor);
            }
        }
    }
}
