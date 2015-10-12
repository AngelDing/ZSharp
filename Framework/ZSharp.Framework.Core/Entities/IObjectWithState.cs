namespace ZSharp.Framework.Entities
{
    public interface IObjectWithState
    {
        ObjectStateType ObjectState { get; set; }
    }

    public enum ObjectStateType
    {
        Added,
        Unchanged,
        Modified,
        Deleted
    }
}
