using System;
using System.Collections.Generic;
/// <summary>
/// 模板方法： 定义一个操作中的算法的骨架，而将一些步骤延迟到子类中。
///          此模式使得子类可以不改变一个算法的结构即可重定义该算法的某些特定步骤。
/// </summary>
namespace Common.BehavioralPatterns
{
    /// <summary>
    /// 模板方法标示
    /// </summary>
    public interface ITemplateMethod
    {
    }

    /// <summary>
    /// 抽象的模版接口
    /// </summary>
    public interface IAbstract : ITemplateMethod
    {
        int Quantity { get; }

        double Total { get; }

        double Average { get; }
    }

    /// <summary>
    /// 定义了算法梗概的抽象类型
    /// </summary>
    public abstract class AbstractBase : IAbstract
    {
        public abstract int Quantity { get; }

        public abstract double Total { get; }

        /// <summary>
        /// 算法梗概
        /// </summary>
        public virtual double Average { get { return Total / Quantity; } }
    }

    /// <summary>
    /// 具体类型
    /// </summary>
    public class ArrayData : AbstractBase
    {
        protected double[] data = new double[3] { 1.1, 2.2, 3.3 };

        public override int Quantity { get { return data.Length; } }

        public override double Total
        {
            get
            {
                double count = 0;
                for (int i = 0; i < data.Length; i++) count += data[i];
                return count;
            }
        }
    }

    /// <summary>
    /// 具体类型
    /// </summary>
    public class ListData : AbstractBase
    {
        protected IList<double> data = new List<double>();

        public ListData() { data.Add(1.1); data.Add(2.2); data.Add(3.3); }

        public override int Quantity { get { return data.Count; } }

        public override double Total
        {
            get
            {
                double count = 0;
                foreach (double item in data) count += item;
                return count;
            }
        }
    }

    public class TemplateMethodClient
    {
        public void Test()
        {
            IAbstract i1 = new ArrayData();
            Console.WriteLine(i1.Average);

            IAbstract i2 = new ListData();
            Console.WriteLine(i2.Average);
        }
    }
}
