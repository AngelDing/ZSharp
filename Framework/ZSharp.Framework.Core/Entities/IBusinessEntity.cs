
namespace ZSharp.Framework.Entities
{
    public interface IBusinessEntity<TKey, TOperator> : IEntity<TKey>, IOperateEntity<TOperator>, IDeleteEntity
    {
    }
}
