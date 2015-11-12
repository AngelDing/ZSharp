
using MazeGame.Composition;

namespace MazeGame.Builder
{
    public class StandardMazeBuilder : MazeBuilder
    {
        private Maze _CurrentMaze;

        public StandardMazeBuilder() { _CurrentMaze = null; }

        public override void BuildMaze()
        {
            _CurrentMaze = new Maze();
        }

        public override Maze GetMaze()
        {
            return _CurrentMaze;
        }

        public override void BuildRoom(int roomNumber)
        {
            if (_CurrentMaze.RoomNumber(roomNumber) != null)
            {
                Room room = new Room(roomNumber);
                _CurrentMaze.AddRoom(room);
                room.SetSide(Sides.East, new Wall());
                room.SetSide(Sides.North, new Wall());
                room.SetSide(Sides.South, new Wall());
                room.SetSide(Sides.West, new Wall());
            }
        }

        public override void BuildDoor(int roomFrom, int roomTo)
        {
            Room room1 = _CurrentMaze.RoomNumber(roomFrom);
            Room room2 = _CurrentMaze.RoomNumber(roomTo);
            Door door = new Door(room1,room2);
            //...
        }
    }
}
