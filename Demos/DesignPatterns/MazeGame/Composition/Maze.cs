using System.Collections.Generic;

namespace MazeGame.Composition
{
    // 这就是我们的迷宫了
    public class Maze
    {
        private IList<Room> m_Rooms = new List<Room>();

        public Room RoomNumber(int roomNumber)
        {
            return (Room)this.m_Rooms[roomNumber];
        }

        public void AddRoom(Room room)
        {
            this.m_Rooms.Add(room);
        }
    }
}
