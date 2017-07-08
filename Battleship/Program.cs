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
		public BattleShip[,] BattleBoard { get; private set; }

		public BattleArea(int width, int height)
		{
			BattleBoard = new BattleShip[width, height];
		}

		public BattleArea(int width, int height, IEnumerable<BattleShip> ships) : this(width, height)
		{
			AddShips(ships);
		}

		public void AddShip(BattleShip ship)
		{

		}
		public void AddShips(IEnumerable<BattleShip> ships)
		{

		}
	}

	enum BattleShipType
	{
		P = 1,
		Q = 2
	}

	class BattleShip
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

	class Game
	{
		public Game(BattleArea playerA, BattleArea playerB)
		{

		}

		public void AutoPlay()
		{
		}
	}
}
