using MazeGame.Composition;

namespace MazeGame.AbstractFactory
{
    public class MazeGame
    {
        public Maze CreateMaze(MazeFactory mazeFactory)
        {
            Maze maze = mazeFactory.MakeMaze();
            Room room1 = mazeFactory.MakeRoom(1);
            Room room2 = mazeFactory.MakeRoom(2);
            Door door = mazeFactory.MakeDoor(room1, room2);
            maze.AddRoom(room1);
            maze.AddRoom(room2);

            room1.SetSide(Sides.North, mazeFactory.MakeWall());
            room1.SetSide(Sides.East, door);
            room1.SetSide(Sides.South, mazeFactory.MakeWall());
            room1.SetSide(Sides.West, mazeFactory.MakeWall());


            room2.SetSide(Sides.North, mazeFactory.MakeWall());
            room2.SetSide(Sides.East, mazeFactory.MakeWall());
            room2.SetSide(Sides.South, mazeFactory.MakeWall());
            room2.SetSide(Sides.West, door);
            return maze;
        }
    }
}
