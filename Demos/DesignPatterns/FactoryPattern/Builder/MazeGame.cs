using MazeGame.Composition;

namespace MazeGame.Builder
{
    public class MazeGame
    {
        public Maze CreateMaze(MazeBuilder mazeBuilder)
        {
            mazeBuilder.BuildMaze();
            mazeBuilder.BuildRoom(1);
            mazeBuilder.BuildRoom(2);
            mazeBuilder.BuildDoor(1, 2);
            return mazeBuilder.GetMaze();
        }
    }
}
