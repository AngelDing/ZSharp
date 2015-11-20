using Common.BehavioralPatterns;
using System;

namespace MementoPattern
{
    /// <summary>
    /// 具体状态类型
    /// </summary>
    public struct Position : ISnapshoot
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// 具体备忘录类型
    /// </summary>
    public class Memento : MementoBase<Position> { }

    /// <summary>
    /// 具体发起人类型
    /// </summary>
    public class Originator : OriginatorBase<Position, Memento>
    {
        /// <summary>
        /// 供客户程序使用的非备忘录相关操作
        /// </summary>
        /// <param name="x"></param>
        public void UpdateX(int x) { base.state.X = x; }
        public void DecreaseX() { base.state.X--; }
        public void IncreaseY() { base.state.Y++; }
        public Position Current { get { return base.state; } }
    }

    public class MementoClient
    {
        public void Test()
        {
            Originator originator = new Originator();
            Console.WriteLine(originator.Current.Y);  //0
            Console.WriteLine(originator.Current.X);  //0
            // 保存发起人刚初始化后的状态
            IMemento<Position> m1 = originator.Memento;

            // 对发起人进行操作，验证状态的修改
            originator.IncreaseY();
            originator.DecreaseX();
            Console.WriteLine(originator.Current.Y);  //1
            Console.WriteLine(originator.Current.X);  //-1

            // 确认备忘录的恢复作用
            originator.Memento = m1;
            Console.WriteLine(originator.Current.Y);  //0
            Console.WriteLine(originator.Current.X);  //0
        }
    }
}
