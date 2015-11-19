using System;
using System.Collections.Generic;

//概述：                                                                                                     
//  迭代器模式(Iterator)：提供一种方法顺序一个聚合对象中各个元素，而又不暴露该对象内部表示。
//实用场合：                                                                                                 
//  1.访问一个聚合对象的内容而无需暴露它的内部表示。
//  2.支持对聚合对象的多种遍历。
//  3.为遍历不同的聚合结构提供一个统一的接口(即，多态迭代)。
//其实.net框架已经准备好了迭代器接口，只需要实现接口就行了, IEumerator支持对非泛型集合的简单迭代

namespace IteratorPattern
{
    public interface IIterator
    {
        object First();

        object Next();

        object Current();

        bool IsDone();
    }

    public interface IAggregate
    {
        IIterator CreateIterator();
    }

    public class ConcreteAggregate : IAggregate
    {
        private IList<object> items = new List<object>();

        public IIterator CreateIterator()
        {
            return new ConcreteIterator(this);
        }

        /// <summary>
        /// 返回聚集总个数
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// 声明一个索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get { return items[index]; }
            set { items.Insert(index, value); }
        }
    }

    public class ConcreteIterator : IIterator
    {
        /// <summary>
        /// 定义了一个具体聚集对象
        /// </summary>
        private ConcreteAggregate aggregate;

        private int current = 0;

        /// <summary>
        /// 初始化对象将具体聚集类传入
        /// </summary>
        /// <param name="aggregate"></param>
        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        /// <summary>
        /// 第一个对象
        /// </summary>
        /// <returns></returns>
        public object First()
        {
            return aggregate[0];
        }

        /// <summary>
        /// 得到聚集的下一对象
        /// </summary>
        /// <returns></returns>
        public object Next()
        {
            object ret = null;
            current++;
            if (current < aggregate.Count)
            {
                ret = aggregate[current];
            }
            return ret;
        }

        /// <summary>
        /// 是否到结尾
        /// </summary>
        /// <returns></returns>
        public bool IsDone()
        {
            return current >= aggregate.Count ? true : false;
        }

        /// <summary>
        /// 返回当前聚集对象
        /// </summary>
        /// <returns></returns>
        public object Current()
        {
            return aggregate[current];
        }
    }

    public class IteratorClient
    {
        public void TestCase1()
        {
            var aggregate = new ConcreteAggregate();
            aggregate[0] = "Apple";
            aggregate[1] = "Orange";
            aggregate[2] = "Strawberry";

            var iterator = new ConcreteIterator(aggregate);

            object item = iterator.First();
            while (iterator.IsDone() == false)
            {
                Console.WriteLine(item);
                item = iterator.Next();
            }
        }
    }
}
