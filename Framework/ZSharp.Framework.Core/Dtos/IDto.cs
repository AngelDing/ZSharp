
using System.ComponentModel.DataAnnotations;

namespace ZSharp.Framework.Dtos
{
    /// <summary>
    /// 標示對象為DTO對象
    /// </summary>
    public interface IDto
    {
    }

    public interface IInputDto : IDto, IValidatableObject
    {
    }

    public interface IOutputDto : IDto
    {
    }
}
