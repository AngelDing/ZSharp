
using System;

namespace ZSharp.Framework.Mvc.IO
{
    public interface IStorageFolder 
    {
        string GetPath();
        string GetName();
        long GetSize();
        DateTime GetLastUpdated();
        IStorageFolder GetParent();
    }
}