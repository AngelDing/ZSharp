
namespace MazeGame.Composition
{
    // 这是一个可以施魔法的门
    public class EnchantedDoor : Door
    {
        public EnchantedDoor(Room roomFrom, Room roomTo) : base(roomFrom, roomTo) { }
    }
}
