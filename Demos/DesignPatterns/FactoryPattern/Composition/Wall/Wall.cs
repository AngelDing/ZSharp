namespace MazeGame.Composition
{
    // 墙是组成迷宫的构件之一，这里是一个很一般的墙（没有炸弹）
    public class Wall : MapSite
    {
        public override string Enter()
        {
            return "This is a Wall.";
        }
    }
}
