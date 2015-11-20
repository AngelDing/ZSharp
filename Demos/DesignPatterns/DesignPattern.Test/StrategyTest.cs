using System;

namespace DesignPattern.Test
{
    public interface MemberStrategy
    {
        /**
         * 计算图书的价格
         * @param booksPrice    图书的原价
         * @return    计算出打折后的价格
         */
        double CalcPrice(double booksPrice);
    }

    public class PrimaryMemberStrategy : MemberStrategy
    {
        public double CalcPrice(double booksPrice)
        {
            Console.WriteLine("对于初级会员的没有折扣");
            return booksPrice;
        }
    }

    public class IntermediateMemberStrategy : MemberStrategy
    {
        public double CalcPrice(double booksPrice)
        {
            Console.WriteLine("对于中级会员的折扣为10%");
            return booksPrice * 0.9;
        }
    }

    public class AdvancedMemberStrategy : MemberStrategy
    {
        public double CalcPrice(double booksPrice)
        {
            Console.WriteLine("对于高级会员的折扣为20%");
            return booksPrice * 0.8;
        }
    }

    public class Price
    {
        //持有一个具体的策略对象
        private MemberStrategy strategy;

        /**
         * 构造函数，传入一个具体的策略对象
         * @param strategy    具体的策略对象
         */
        public Price(MemberStrategy strategy)
        {
            this.strategy = strategy;
        }

        /**
         * 计算图书的价格
         * @param booksPrice    图书的原价
         * @return    计算出打折后的价格
         */
        public double quote(double booksPrice)
        {
            return this.strategy.CalcPrice(booksPrice);
        }
    }

    public class StrategyClient
    {
        public void Test()
        {
            //选择并创建需要使用的策略对象
            var strategy = new AdvancedMemberStrategy();
            //创建环境
            var price = new Price(strategy);
            //计算价格
            double quote = price.quote(300);
            Console.WriteLine("图书的最终价格为：" + quote);
        }
    }
}