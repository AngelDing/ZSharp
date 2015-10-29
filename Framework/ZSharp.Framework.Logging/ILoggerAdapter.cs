using System;

namespace ZSharp.Framework.Logging
{
    public interface ILoggerAdapter
    {
        ILogger GetLogger(Type type);

        /// <summary>
        /// 根據名稱獲取Logger，除了TraceLogger名稱必須與配置的source name相同外，其他無特別要求
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ILogger GetLogger(string name);
    } 
}
 