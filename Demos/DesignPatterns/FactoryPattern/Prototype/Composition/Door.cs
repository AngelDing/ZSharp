using System;
using MazeGame.Composition;

namespace MazeGame.Prototype
{
    public class Door : MapSite
    {
        private Room _Room1;
        private Room _Room2;
           
        public Door(Room room1, Room room2)
        {
            _Room1 = room1;
            _Room2 = room2;
        }

        public Door(Door door)
        {
            _Room1 = door._Room1;
            _Room2 = door._Room2;
        }

        public virtual void Initialize(Room room1, Room room2)
        {
            _Room1 = room1;
            _Room2 = room2;
        }

        public Door Clone()
        {
            return new Door(this);
        }

        public override string Enter()
        {
            throw new NotImplementedException();
        }
    }
}
