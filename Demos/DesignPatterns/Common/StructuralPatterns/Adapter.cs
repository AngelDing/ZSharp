/// <summary>
/// 適配器模式，別名包装器（Wrapper）
/// </summary>

namespace Common
{
    public class Adapter1
    {
        private Adaptee1 adaptee = new Adaptee1();    // Adaptee对象

        public void Request()
        {
            // 其他操作
            adaptee.SpecifiedRequest1();    // 调用Adaptee
        }
    }

    public class Adapter2
    {
        private Adaptee2 adaptee = new Adaptee2();  

        public void Request()
        {
            adaptee.SpecifiedRequest2();    
        }
    }

    public interface ITarget
    {
        void Request();
    }

    public class Adaptee1
    {
        public void SpecifiedRequest1() { }
    }

    public class Adaptee2
    {
        public void SpecifiedRequest2() { }
    }
}
