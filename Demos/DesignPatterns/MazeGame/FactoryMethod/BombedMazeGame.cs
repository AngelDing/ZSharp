using MazeGame.Composition;

namespace MazeGame.FactoryMethod
{
    // 创建带炸弹迷宫
    public class BombedMazeGame : MazeGame
    {
        public override Wall MakeWall()
        {
            return new BombedWall();
        }

        public override Room MakeRoom(int n)
        {
            return new BombedRoom(n);
        }
    }
}
