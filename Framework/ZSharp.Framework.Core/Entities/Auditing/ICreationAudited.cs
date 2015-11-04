namespace ZSharp.Framework.Entities
{
    public interface ICreationAudited<TUser> : IHasCreationTime
    {
        TUser CreatedBy { get; set; }
    }
}