using System;
using System.Collections.Generic;

/// <summary>
/// 組合模式： 将对象组合成树形结构以表示 “部分-整体” 的层次结构。
///          Composite 使得用户对于单个对象和组合对象的使用具有一致性。
/// </summary>
namespace Common
{
    public abstract class Component
    {
        /// <summary>
        /// 保存的子节点
        /// </summary>
        protected IList<Component> children;

        /// <summary>
        /// Leaf和Composite的共同特征. 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 其实只有Composite类型才需要真正实现的功能
        /// </summary>
        /// <param name="child"></param>
        public virtual void Add(Component child) { children.Add(child); }

        public virtual void Remove(Component child) { children.Remove(child); }

        public virtual Component this[int index] { get { return children[index]; } }

        /// <summary>
        /// 演示用的补充方法：实现迭代器，并且对容器对象实现隐性递归
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetNameList()
        {
            yield return Name;
            if ((children != null) && (children.Count > 0))
            {
                foreach (Component child in children)
                {
                    foreach (string item in child.GetNameList())
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// 演示用的补充方法：实现迭代器，并且对容器对象实现隐性递归
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Component> Enumerate(IMatchRule rule)
        {
            if ((rule == null) || (rule.IsMatch(this)))
            {
                yield return this;
            }
            if ((children != null) && (children.Count > 0))
            {
                foreach (Component child in children)
                {
                    foreach (Component item in child.Enumerate(rule))
                    {
                        if ((rule == null) || (rule.IsMatch(item)))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        public virtual IEnumerable<Component> Enumerate()
        {
            return Enumerate(null);
        }
    }

    public interface IMatchRule
    {
        bool IsMatch(Component target);
    }

    public class LeafMatchRule : IMatchRule
    {
        public bool IsMatch(Component target)
        {
            if (target == null) return false;
            {
                return (target.GetType().IsAssignableFrom(typeof(Leaf))) ? true : false;
            }
        }
    }

    public class Leaf : Component
    {
        /// <summary>
        /// 明确声明不支持此类操作
        /// </summary>
        /// <param name="child"></param>
        public override void Add(Component child) { throw new NotSupportedException(); }

        public override void Remove(Component child) { throw new NotSupportedException(); }

        public override Component this[int index] { get { throw new NotSupportedException(); } }
    }

    public class Composite : Component
    {
        public Composite() { base.children = new List<Component>(); }
    }

    /// <summary>
    /// 由于组合形式对象的特殊性，这个工厂同时具有工厂方法模式和抽象工厂模式的特点：
    /// T : Component, new() 体现出工厂方法的特点
    /// 但Create<T>其实相当于CreateComposite()、CreateLeaf()...的作用，体现抽象工厂特点。
    /// </summary>
    public class ComponentFactory
    {
        public Component Create<T>(string name) where T : Component, new()
        {
            T instance = new T();
            instance.Name = name; 
            return instance;
        }

        /// <summary>
        /// 连贯性方法（Fluent Method）: 直接向某个节点下增加新的节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Component Create<T>(Component parent, string name)
            where T : Component, new()
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (!(parent is Composite))
            {
                throw new Exception("non-somposite type");
            }
            Component instance = Create<T>(name);
            parent.Add(instance);
            return instance;
        }
    }
}
