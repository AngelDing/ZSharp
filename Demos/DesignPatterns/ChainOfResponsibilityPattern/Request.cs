
using Common.BehavioralPatterns;

namespace ChainOfResponsibilityPattern
{
    /// <summary>
    /// 请求对象
    /// </summary>
    public class Request : IRequest
    {
        public Request(double price, PurchaseType type)
        {
            this.Price = price;
            this.EnumType = type;
        }

        public double Price { get; set; }

        public object EnumType { get; set; }
    }
}
