
namespace ZSharp.Framework.Domain
{
    /// <summary>
    /// Defines that the implementor can create memento objects (snapshots), that can be used to recreate the original state.
    /// </summary>
    public interface IMementoOriginator
    {
        /// <summary>
        /// Saves the object's state to an opaque memento object (a snapshot) that can be used to restore the state.
        /// </summary>
        /// <returns>An opaque memento object that can be used to restore the state.</returns>
        IMemento SaveToMemento();
    }

    /// <summary>
    /// An opaque object that contains the state of another object (a snapshot) and can be used to restore its state.
    /// </summary>
    public interface IMemento
    {
        /// <summary>
        /// The version of the <see cref="IEventSourced"/> instance.
        /// </summary>
        int Version { get; }
    }
}
