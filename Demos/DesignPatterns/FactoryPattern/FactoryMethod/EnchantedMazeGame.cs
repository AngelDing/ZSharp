using MazeGame.Composition;

namespace MazeGame.FactoryMethod
{
    // 创建带魔法的迷宫
    public class EnchantedMazeGame : MazeGame
    {
        private Spell m_spell;

        public EnchantedMazeGame(Spell spell)
        {
            this.m_spell = spell;
        }

        public override Door MakeDoor(Room r1, Room r2)
        {
            return new EnchantedDoor(r1, r2);
        }

        public override Room MakeRoom(int n)
        {
            return new EnchantedRoom(n, this.m_spell);
        }
    }
}
