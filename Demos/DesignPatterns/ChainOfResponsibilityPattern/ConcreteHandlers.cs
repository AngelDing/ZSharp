

using Common.BehavioralPatterns;

namespace ChainOfResponsibilityPattern
{
    /// <summary>
    /// 具体操作类型
    /// </summary>
    public class InternalHandler : BaseHandler
    {
        public InternalHandler() : base(PurchaseType.Internal)
        {
        }
        public override void Process(IRequest request)
        {
            var req = request as Request;
            req.Price *= 0.6;
        }
    }

    public class MailHandler : BaseHandler
    {
        public MailHandler() : base(PurchaseType.Mail) { }
        public override void Process(IRequest request)
        {
            var req = request as Request;
            req.Price *= 1.3;
        }
    }

    public class DiscountHandler : BaseHandler
    {
        public DiscountHandler() : base(PurchaseType.Discount) { }
        public override void Process(IRequest request)
        {
            var req = request as Request;
            req.Price *= 0.9;
        }
    }

    public class RegularHandler : BaseHandler
    {
        public RegularHandler() : base(PurchaseType.Regular) { }
        public override void Process(IRequest request) { }
    }
}
