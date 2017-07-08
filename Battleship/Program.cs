using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new BattleArea(5, 5);
            player1.AddShips(new[]
			{
				new BattleShip(BattleShipType.P, 1, 1),
				new BattleShip(BattleShipType.P, 1, 1),
			});

            var game = new Game(new BattleArea(5, 5), new BattleArea(5, 5));
            game.AutoPlay();
        }
    }

    class BattleArea
    {
        public BattleBoard Board { get; private set; }

        public BattleArea(int width, int height)
        {
            Board = new BattleBoard(width, height);
        }

        public BattleArea(int width, int height, IEnumerable<BattleShip> ships)
            : this(width, height)
        {
            AddShips(ships);
        }

        public void AddShip(BattleShip ship)
        {
            Board.AddShip(ship);
        }
        public void AddShips(IEnumerable<BattleShip> ships)
        {
            Board.AddShips(ships);
        }
    }

    enum BattleShipType
    {
        P = 1,
        Q = 2
    }

    class BattleShip : IBattleShip
    {
        public BattleShipType Type { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public BattleShip(BattleShipType type, int height, int width)
        {
            Type = type;
            Height = height;
            Width = width;
        }
    }

    class BattleBoard : IBattleBoard
    {
        private IBattleShip[,] board { get; set; }

        public BattleBoard(int width, int height)
        {
            board = new BattleShip[width, height];
        }

        public IBattleShip this[int i, int j]
        {
            get
            {
                return board[i, j];
            }
            set
            {
                board[i, j] = value;
            }
        }

        public bool AddShip(IBattleShip ship)
        {
            return false;
        }

        public bool AddShips(IEnumerable<IBattleShip> ship)
        {
            return false;
        }
    }

    class Game
    {
        public Game(BattleArea playerA, BattleArea playerB)
        {

        }

        public void AutoPlay()
        {
        }
    }


    interface IBattleShip
    {
        BattleShipType Type { get; }
        int Height { get; }
        int Width { get; }
    }

    interface IBattleBoard
    {
        IBattleShip this[int i, int j] { get; }
        bool AddShip(IBattleShip ship);
        bool AddShips(IEnumerable<IBattleShip> ship);
    }
}
