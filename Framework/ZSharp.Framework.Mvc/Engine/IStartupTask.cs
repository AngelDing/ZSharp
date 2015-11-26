namespace ZSharp.Framework.Mvc
{
    public interface IStartupTask 
    {
        void Execute();

        int Order { get; }
    }
}
