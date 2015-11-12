using MazeGame.Composition;

namespace MazeGame.AbstractFactory
{
    public class EnchantedMazeFactory : MazeFactory
    {
        public override Room MakeRoom(int roomNumber)
        {
            return new EnchantedRoom(roomNumber, new Spell());
        }

        public override Door MakeDoor(Room room1, Room room2)
        {
            return new EnchantedDoor(room1, room2);
        }
    }
}
