/// <summary>
/// 解析器（不常用） ： 给定一个语言，定义它的文法的一种表示，并定义一个解释器，这个解释器使用该表示来解释语言中的句子。
/// 这个Pattern用在Compiler的 Language Parser最合适。他可以反覆递归，最终分析好一个语言。
/// 语法就象: 算术表达式 ＝ 算术表达式 ＋／－？＊／／ 数字
/// 然后构造一个算术表达式类和数字类，在算术表达式类中递归引用自己和数字类。最终能够解开任何的语言。
/// 在Gang Of Four的网页上是通过LinkedList来处理这个例子。 LinkedList就是先准备好一堆Class，
/// 一个一个拿过来用在Context上。比较死板，只能处理很少的情况。在Compiler中最常用的是递归。
/// 反复递归就是有一万层括号也能解开，但是用LinkedList就会受限制了。"
/// </summary>
namespace Common.BehavioralPatterns
{
    /// <summary>
    /// 表示所有表达式的抽象接口
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// 用Context负责保存中间结果
        /// </summary>
        /// <param name="ctx"></param>
        object Interpret(IContext ctx);
    }

    /// <summary>
    /// 用于保存解析过程的中间结果以及当前执行的操作符
    /// </summary>
    public interface IContext
    {
    }
}
