using System.Transactions;

namespace ZSharp.Framework.Infrastructure
{
    public static class TransactionScopeFactory
    {
        public static TransactionScope CreateReadCommittedScope()
        {
            var options = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            var scope = new TransactionScope(TransactionScopeOption.Required, options);
            return scope;
        }
    }
}
