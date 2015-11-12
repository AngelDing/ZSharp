
namespace MazeGame.Composition
{
    // 房间是组成迷宫的基本单位
    public class Room : MapSite
    {       
        public override string Enter()
        {
            return "This is a Room.";
        }

        // 构造函数，为了可以区分房间我们给每一个房间加一个标示
        public Room(int roomNumber)
        {
            this.m_roomNumber = roomNumber;
        }

        // 设置房子的面，房子有4个面组成，因为我们现在不知道每个面
        // 的具体类型（门？墙？）所以我们用MapSite类型。
        public void SetSide(Sides side, MapSite sideMap)
        {
            this.m_side[(int)side] = sideMap;
        }

        // 得到指定的面，同样我们不知道得到的是哪一个面
        // 所以我们用MapSite返回结构
        public MapSite GetSide(Sides side)
        {
            return this.m_side[(int)side];
        }

        // 一些私有成员 房号
        protected int m_roomNumber;

        // 房子有4个面
        protected const int m_Sides = 4;

        // 用一个1维的MapSite数组存储未知类型的面（墙或门）
        protected MapSite[] m_side = new MapSite[m_Sides];
    }
}
