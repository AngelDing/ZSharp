using System;
using MazeGame.Composition;

namespace MazeGame.Prototype
{
    public class Wall : MapSite
    {
        public Wall()
        {
        }

        public Wall(Wall wall) { }

        public virtual Wall Clone() { return new Wall(this); }

        public override string Enter()
        {
            throw new NotImplementedException();
        }
    }
}
