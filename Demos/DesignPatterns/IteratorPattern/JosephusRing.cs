using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IteratorPattern
{
    /// <summary>
    /// 约瑟夫环
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class JosephusRing<T> 
    {
        protected IList<T> ringItems;
        private int currentPosition;

        /// <summary>
        /// 循环列表
        /// </summary>
        /// <param name="items">供循环的列表项</param>
        public JosephusRing(int length, int startPosition)
        {
            this.currentPosition = startPosition;
            CreateRing(length);    //将环的构造放到子类来完成，应该算是工厂方法模式吧。
        }

        public void NextStep()
        {
            currentPosition = Next(currentPosition);
            Process(currentPosition);
            ringItems.RemoveAt(currentPosition);        //出列是不变的
        }

        public void RunToEnd()
        {
            while (ringItems.Count > 0)
            {
                NextStep();
            }
        }

        /// <summary>
        /// 肯定有方法找到下一个位置，但是怎么找不确定
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        protected abstract int Next(int current);

        /// <summary>
        /// 找到后要进行一些处理，至于是跳三跳还是戴红花那就不知道了
        /// </summary>
        /// <param name="current"></param>
        protected abstract void Process(int current);

        protected abstract void CreateRing(int length);
    }

    public class JosephusRingInt32 : JosephusRing<int>
    {
        public JosephusRingInt32(int length, int startPosition)
            : base(length, startPosition)
        {
        }

        protected override int Next(int current) //从1数到3
        {
            return (current + 2) % base.ringItems.Count;
        }

        protected override void Process(int current) //只是简单地输出
        {
            Console.WriteLine(base.ringItems[current].ToString());
        }

        //构造环的过程只能放到派生类来完成，如果是一圈阿猫，或许你就得给它们依次起个名字
        protected override void CreateRing(int length)
        {
            int[] r = new int[length];
            for (int i = 0; i < length; ++i)
                r[i] = i + 1;
            base.ringItems = new List<int>(r);
        }
    }
}
