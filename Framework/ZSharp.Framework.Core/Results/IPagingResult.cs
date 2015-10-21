using System.Collections.Generic;
using ZSharp.Framework.Common;

namespace ZSharp.Framework.Results
{
    public interface IPagingResult<T> : IPaging
    {
        IEnumerable<T> Datas { get; set; }
    }
}
