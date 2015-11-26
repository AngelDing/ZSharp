﻿using System.Collections.Generic;
using System.IO;

namespace ZSharp.Framework.Mvc.IO
{

    /// <summary>
    /// Abstraction over the virtual files/directories of a web site.
    /// </summary>
    public interface IWebSiteFolder
    {
        IEnumerable<string> ListDirectories(string virtualPath);
        IEnumerable<string> ListFiles(string virtualPath, bool recursive);

        bool FileExists(string virtualPath);
        string ReadFile(string virtualPath);
        void CopyFileTo(string virtualPath, Stream destination);
    }

}
