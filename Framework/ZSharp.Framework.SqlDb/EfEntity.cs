using System;
using ZSharp.Framework.Entities;

namespace ZSharp.Framework.SqlDb
{
    /// <summary>
    /// 定义统一IEntity<TKey>的接口時使用，即所有繼承此類的Entity均使用Id作為主鍵字段
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    [Serializable]
    public abstract class EfEntity<Tkey> : BaseEntity<Tkey>
    {
    }  
}
