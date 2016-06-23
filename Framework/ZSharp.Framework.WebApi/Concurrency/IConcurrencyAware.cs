
namespace ZSharp.Framework.WebApi
{
    public interface IConcurrencyAware
    {
        string ConcurrencyVersion { get; set; }
    }
}