
namespace MazeGame.Builder
{
    /// <summary>
    /// 这个builder类只用来进行计数
    /// </summary>
    public class CountingMazeBuilder : MazeBuilder
    {
        private int _Doors;
        private int _Rooms;
        public CountingMazeBuilder()
        {
            _Doors = 0;
            _Rooms = 0;
        }
        public override void BuildRoom(int roomNumber)
        {
            _Rooms++;
        }
        public override void BuildDoor(int roomFrom, int roomTo)
        {

            _Doors++;
        }
        public void GetCounts(ref int rooms, ref int doors)
        {
            rooms = _Rooms;
            doors = _Doors;
        }
    }
}
