using System;

namespace ZSharp.Framework.Entities
{
    public interface IOperateEntity<TOperator>
    {
        DateTimeOffset CreatedDate { get; set; }

        TOperator CreatedBy { get; set; }

        DateTimeOffset UpdatedDate { get; set; }

        TOperator UpdatedBy { get; set; }
    }
}
