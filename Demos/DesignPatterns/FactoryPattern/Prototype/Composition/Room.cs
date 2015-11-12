using MazeGame.Composition;
using System.Collections.Generic;
using System;

namespace MazeGame.Prototype
{
    public class Room : MapSite
    {
        private int _RoomNumber;
        private List<MapSite> _MapSiteList;

        public Room(int roomNumber)
        {
            _RoomNumber = roomNumber;
            _MapSiteList = new List<MapSite>();
        }

        public MapSite GetSide(Sides side)
        {
            switch (side)
            {
                case Sides.North:
                    return _MapSiteList[0];
                case Sides.South:
                    return _MapSiteList[1];
                case Sides.West:
                    return _MapSiteList[2];
                case Sides.East:
                    return _MapSiteList[3];
            }
            return null;
        }
        public void SetSide(Sides side, MapSite mapSite)
        {
            switch (side)
            {
                case Sides.North:
                    _MapSiteList[0] = mapSite;
                    break;
                case Sides.South:
                    _MapSiteList[1] = mapSite;
                    break;
                case Sides.West:
                    _MapSiteList[2] = mapSite;
                    break;
                case Sides.East:
                    _MapSiteList[3] = mapSite;
                    break;
            }
        }

        public override string Enter()
        {
            throw new NotImplementedException();
        }
    }
}
