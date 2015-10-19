using System.Transactions;

namespace ZSharp.Framework.Infrastructure
{
    public class ReadUncommittedScope : DisposableObject
    {
        private bool autoComplete;

        public ReadUncommittedScope(bool autoComplete = true)
        {
            this.autoComplete = autoComplete;
            var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted };
            ReadUnCommitedTransactionScope = new TransactionScope(TransactionScopeOption.Required, options);
        }

        public TransactionScope ReadUnCommitedTransactionScope
        {
            get;
            private set;
        }

        public void Complete()
        {
            ReadUnCommitedTransactionScope.Complete();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (autoComplete)
                    {
                        Complete();
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    ReadUnCommitedTransactionScope.Dispose();
                }
            }
        }
    }
}
