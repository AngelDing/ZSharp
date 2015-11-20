using Common.BehavioralPatterns;
using System;

namespace DesignPattern.Test
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

    /// <summary>
    /// 此用例用的是结构类型，是按值复制储存的；
    /// 如果用的是类类型，则在存储快照时需要深度克隆，否则，修改前后数据是一致的
    /// </summary>
    public class MementoClient
    {
        public void Test()
        {
            Originator originator = new Originator();
            Console.WriteLine(originator.Current.Y);  //0
            Console.WriteLine(originator.Current.X);  //0
            // 保存发起人刚初始化后的状态
            var caretaker = originator.Memento;

            // 对发起人进行操作，验证状态的修改
            originator.IncreaseY();
            originator.DecreaseX();
            Console.WriteLine(originator.Current.Y);  //1
            Console.WriteLine(originator.Current.X);  //-1

            // 确认备忘录的恢复作用
            originator.Memento = caretaker;
            Console.WriteLine(originator.Current.Y);  //0
            Console.WriteLine(originator.Current.X);  //0
        }
    }
}
