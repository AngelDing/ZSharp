

namespace MazeGame.Prototype
{
    public class BombedWall : Wall
    {
        public BombedWall(BombedWall bombedWall)
            : base(bombedWall)
        {
        }

        public override Wall Clone()
        {
            return new BombedWall(this);
        }
    }
}
