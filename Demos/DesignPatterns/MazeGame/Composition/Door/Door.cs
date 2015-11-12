

namespace MazeGame.Composition
{
    // 门也是迷宫的组成部分之一，这也是一个很普通的门（不能施魔法）
    public class Door : MapSite
    {
        // 这是一些私有的变量
        private Room m_Room1;
        private Room m_Room2;
        // 描述门的状态默认所有的新门都是关的
        private bool m_IsOpen = false;

        // 门是在两个房子之间的构件所以构造函数包含两个房子
        // 说明是哪两个房子之间的门
        public Door(Room roomFrom, Room roomTo)
        {
            this.m_Room1 = roomFrom;
            this.m_Room2 = roomTo;
        }

        public override string Enter()
        {
            return "This is a Door.";
        }       

        // 让我们有机会可以从门进入另一个房子，将会得到
        // 另一个房子的引用
        public Room OtherSideFrom(Room roomFrom)
        {
            if (this.m_Room1 == roomFrom)
            {
                return this.m_Room2;
            }
            else
            {
                return this.m_Room1;
            }
        }

        // 提供一个公共访问的访问器
        public bool IsOpen
        {
            set { this.m_IsOpen = value; }
            get { return this.m_IsOpen; }
        }
    }
}
