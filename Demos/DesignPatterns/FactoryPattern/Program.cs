using MazeGame.Composition;
using System;

namespace MazeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractFactoryGame();

            FactoryMethodGame();

            Console.ReadLine();
        }

        private static void AbstractFactoryGame()
        {
            var mazeGame = new AbstractFactory.MazeGame();
            var bomedMazeFactory = new AbstractFactory.BombedMazeFactory();
            var enchantedMazeFactory = new AbstractFactory.EnchantedMazeFactory();

            //建造一个有炸弹的迷宫
            mazeGame.CreateMaze(bomedMazeFactory);
            //建造一个有魔力的迷宫
            mazeGame.CreateMaze(enchantedMazeFactory);
        }

        private static void FactoryMethodGame()
        {
            FactoryMethod.MazeGame mazeGame = null;
            //建造一个有炸弹的迷宫
            mazeGame = new FactoryMethod.BombedMazeGame();
            mazeGame.CreateMaze();

            //建造一个有魔力的迷宫
            mazeGame = new FactoryMethod.EnchantedMazeGame(new Spell());
            mazeGame.CreateMaze();
        }

        private static void BuilderGame()
        {
            Maze maze = new Maze();
            var mazeGame = new Builder.MazeGame();
            var standardMazeBuilder = new Builder.StandardMazeBuilder();
            mazeGame.CreateMaze(standardMazeBuilder);
            maze = standardMazeBuilder.GetMaze();
        }
    }
}
