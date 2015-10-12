using System;

namespace ZSharp.Framework.SqlDb
{
    public class EfRepositoryException : Exception
    {
        public EfRepositoryException(string message)
            : base(message)
        {
        }
    }
}
