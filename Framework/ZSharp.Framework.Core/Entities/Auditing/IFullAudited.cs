namespace ZSharp.Framework.Entities
{
    public interface IFullAudited<TUser> : IAudited<TUser>, IDeletionAudited<TUser>
    {
    }
}