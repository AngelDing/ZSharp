namespace ZSharp.Framework.Entities
{
    public interface IAudited<TUser> : ICreationAudited<TUser>, IModificationAudited<TUser>
    {
    }
}