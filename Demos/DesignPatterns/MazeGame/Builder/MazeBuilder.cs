using MazeGame.Composition;

namespace MazeGame.Builder
{
    public class MazeBuilder
    {
        public virtual void BuildMaze() { }

        public virtual void BuildRoom(int roomNumber) { }

        public virtual void BuildDoor(int roomFrom, int roomTo) { }

        public virtual Maze GetMaze() { return new Maze(); }
    }
}
