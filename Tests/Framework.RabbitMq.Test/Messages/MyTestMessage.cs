
namespace Framework.RabbitMq.Test
{
    public class MyTestMessage
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

        public decimal Total
        {
            get
            {
                return Price * Qty;
            }
        }
    }
}
