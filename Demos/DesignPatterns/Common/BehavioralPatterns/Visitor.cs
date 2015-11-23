

namespace Common.BehavioralPatterns
{
    public interface IVisitor
    {
        void Visit(IVisitorElement element);
    }

    public interface IVisitorElement
    {
        void Accept(IVisitor visitor);
    }
}
