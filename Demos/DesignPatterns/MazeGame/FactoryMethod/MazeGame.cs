using MazeGame.Composition;

namespace MazeGame.FactoryMethod
{
    // 实现了一些工厂方法的Creator类
    public class MazeGame
    {
        // 要返回给Client的对象
        private Maze m_Maze = null;

        // 一个访问器用来得到maze
        public Maze Maze
        {
            get
            {
                if (this.m_Maze == null)
                {
                    this.m_Maze = CreateMaze();
                }
                return this.m_Maze;
            }
        }

        // 以下就是一些工厂方法创建迷宫的每个个构件
        public virtual Maze MakeMaze()
        {
            return new Maze();
        }

        public virtual Room MakeRoom(int id)
        {
            return new Room(id);
        }

        public virtual Wall MakeWall()
        {
            return new Wall();
        }

        public virtual Door MakeDoor(Room room1, Room room2)
        {
            return new Door(room1, room2);
        }

        // 创建迷宫
        public Maze CreateMaze()
        {
            Maze maze = MakeMaze();

            // 创建门和房间
            Room room1 = MakeRoom(1);
            Room room2 = MakeRoom(2);
            Door theDoor = MakeDoor(room1, room2);

            // 将房间添加到迷宫里面
            maze.AddRoom(room1);
            maze.AddRoom(room2);

            // 设置room1的面
            room1.SetSide(Sides.North, MakeWall());
            room1.SetSide(Sides.East, theDoor);
            room1.SetSide(Sides.South, MakeWall());
            room1.SetSide(Sides.West, MakeWall());

            // 设置room2的面
            room2.SetSide(Sides.North, MakeWall());
            room2.SetSide(Sides.East, MakeWall());
            room2.SetSide(Sides.South, MakeWall());
            room2.SetSide(Sides.West, theDoor);

            return maze;
        }
    }
}
