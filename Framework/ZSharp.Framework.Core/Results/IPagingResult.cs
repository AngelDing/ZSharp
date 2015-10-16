using System.Collections.Generic;
using ZSharp.Framework.ValueObjects;

namespace ZSharp.Framework.Results
{
    public interface IPagingResult<T> : IPaging
    {
        IEnumerable<T> Datas { get; set; }
    }
}
