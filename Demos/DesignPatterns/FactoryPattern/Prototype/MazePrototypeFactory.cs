
namespace MazeGame.Prototype
{
    public class MazePrototypeFactory : MazeFactory
    {
        private Maze _Maze;
        private Wall _Wall;
        private Room _Room;
        private Door _Door;

        public MazePrototypeFactory(Maze maze, Wall wall, Room room, Door door)
        {
            _Door = door;
            _Room = room;
            _Wall = wall;
            _Maze = maze;
        }

        public override Wall MakeWall()
        {
            return _Wall.Clone();
        }

        public override Door MakeDoor(Room room1, Room room2)
        {
            Door door = _Door.Clone();
            door.Initialize(room1, room2);
            return door;
        }
    }
}
