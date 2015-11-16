/// <summary>
/// 代理模式 ： 为其他对象提供一种代理以控制对这个对象的访问；是实现按需实例化的利器。
/// </summary>
namespace Common
{
    /// <summary>
    /// 定义客户程序需要的抽象类型
    /// </summary>
    public interface ISubject
    {
        string Request();
    }

    /// <summary>
    /// 具体实现客户程序需要的类型
    /// </summary>
    public class RealSubject : ISubject
    {
        public string Request()
        {
            return "from real subject";
        }
    }

    /// <summary>
    /// 代理类型，他知道如何满足客户程序的要求，同时知道具体类型如何访问
    /// </summary>
    public class Proxy : ISubject
    {
        private ISubject realSubject = null;

        public string Request()
        {
            if (realSubject == null)
            {
                LoadRealSubject();
            }

            return realSubject.Request();
        }

        private void LoadRealSubject()
        {
            // do some heavy things
            realSubject = new RealSubject();
        }
    }

    public class ProxyClient
    {
        public void TestCase1()
        {
            var subject = new Proxy();
            subject.Request();
        }
    }
}
