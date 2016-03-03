namespace ZSharp.Framework.Entities
{
    public interface IObjectWithState
    {
        ObjectStateType ObjectState { get; set; }
    }

    /// <summary>
    /// 采用Repo的Insert或者Delete时，请不要单独给对象的ObjectStatte手动赋值
    /// 只有采用Repo的Update方法时，如果主表有修改，同时子表有新增，修改，或者删除时，此时手动赋值才有意义
    /// </summary>
    public enum ObjectStateType : byte
    {
        Unchanged = 0,
        Added = 1,
        PartialModified = 2,
        Modified = 3,
        Deleted = 4
    }
}
