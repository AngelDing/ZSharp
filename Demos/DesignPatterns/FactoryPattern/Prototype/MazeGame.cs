
namespace MazeGame.Prototype
{
    public class MazeGame
    {
        public MazeGame()
        { }

        public Maze CreateMaze(MazeFactory mazeFactory)
        {
            Maze maze = mazeFactory.MakeMaze();
            Room room1 = mazeFactory.MakeRoom(1);
            Room room2 = mazeFactory.MakeRoom(2);
            Door door = mazeFactory.MakeDoor(room1, room2);
            maze.AddRoom(room1);
            maze.AddRoom(room2);

            room1.SetSide(Composition.Sides.North, mazeFactory.MakeWall());
            room1.SetSide(Composition.Sides.East, door);
            room1.SetSide(Composition.Sides.South, mazeFactory.MakeWall());
            room1.SetSide(Composition.Sides.West, mazeFactory.MakeWall());


            room2.SetSide(Composition.Sides.North, mazeFactory.MakeWall());
            room2.SetSide(Composition.Sides.East, mazeFactory.MakeWall());
            room2.SetSide(Composition.Sides.South, mazeFactory.MakeWall());
            room2.SetSide(Composition.Sides.West, door);
            return maze;
        }
    }
}
