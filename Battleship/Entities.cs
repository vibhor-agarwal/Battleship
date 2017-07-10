using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Constants;
using Battleship.Contracts;

namespace Battleship.Entities
{
	class PlayerBattleArea
	{
		public string Id { get; private set; }
		public BattleBoard Board { get; private set; }
		public Queue<Attack> AttackSequence { get; private set; }
		public int ShipsAlive { get { return shipInventory.Count(x => x.Health != ShipHealth.Destroyed); } }

		private List<IBattleShip> shipInventory { get; set; }

		public PlayerBattleArea(string id, int height, int width)
		{
			Id = id;
			Board = new BattleBoard(height, width);
			shipInventory = new List<IBattleShip>();
		}

		#region Configuration
		public void AddShip(BattleShip ship, BattleBoard.Position position)
		{
			if (Board.AddShip(ship, position))
			{
				shipInventory.Add(ship);
			}
		}

		public void AddAttackSequence(List<Attack> attackSequence)
		{
			AttackSequence = new Queue<Attack>(attackSequence.Count);
			attackSequence.ForEach(x => AttackSequence.Enqueue(x));
		}
		#endregion

		#region Gameplay
		public bool HandleAttack(Attack attack)
		{
			return Board.HandleAttack(attack);
		}
		#endregion
	}

	class BattleShip : IBattleShip
	{
		public BattleShipType Type { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }
		public int Size { get; private set; }
		public int RemainingHits { get; private set; }
		public List<IShipPart> Parts { get; private set; }
		public ShipHealth Health
		{
			get
			{
				return Parts.All(x => x.Health == ShipHealth.Destroyed) ?
						ShipHealth.Destroyed : Parts.Any(x => x.Health == ShipHealth.Destroyed) ?
						ShipHealth.Hit : ShipHealth.Fresh;
			}
		}

		public BattleShip(BattleShipType type, int width, int height)
		{
			Type = type;
			Height = height;
			Width = width;
			Size = Height * Width;
			RemainingHits = Type == BattleShipType.P ? 1 : 2;
			BuildParts();
		}

		private void BuildParts()
		{
			Parts = new List<IShipPart>();
			for (int i = 0; i < Size; i++)
			{
				Parts.Add(new ShipPart(this));
			}
		}
	}

	class ShipPart : IShipPart
	{
		public IBattleShip Parent { get; private set; }
		public ShipHealth Health { get; private set; }
		public int RemainingHits { get; private set; }
		public BattleShipType Type { get { return Parent.Type; } }

		public ShipPart(IBattleShip parent)
		{
			Parent = parent;
			RemainingHits = parent.Type == BattleShipType.P ? 1 : 2;
			Health = ShipHealth.Fresh;
		}

		public bool AbsorbHit()
		{
			RemainingHits--;
			Health = RemainingHits > 0 ? ShipHealth.Hit : ShipHealth.Destroyed;
			return RemainingHits > 0 ? true : false;
		}
	}

	partial class BattleBoard : IBattleBoard
	{
		private IShipPart[,] board { get; set; }

		public BattleBoard(int height, int width)
		{
			board = new ShipPart[height, width];
		}

		public IShipPart this[int row, int col]
		{
			get { return board[row, col]; }
			set { board[row, col] = value; }
		}

		public bool AddShip(IBattleShip ship, IBoardPosition position)
		{
			var y = position.Row - 1;
			var x = position.Column - 1;
			var counter = 0;

			for (int i = y; i < y + ship.Height; i++)
			{
				for (int j = x; j < x + ship.Width; j++, counter++)
				{
					this[i, j] = ship.Parts[counter];
				}
			}
			return true;
		}

		#region Gameplay
		public bool HandleAttack(Attack attack)
		{
			bool attackHandled = true;
			var shipPart = this[attack.Row - 1, attack.Column - 1];
			if (shipPart != null)
			{
				attack.Result = AttackResult.Hit;
				if (!shipPart.AbsorbHit())
				{
					this[attack.Row - 1, attack.Column - 1] = null;
					attackHandled = false;
				}
			}
			else
			{
				attack.Result = AttackResult.Miss;
			}
			return attackHandled;
		}
		#endregion
	}

	partial class BattleBoard : IBattleBoard
	{
		public class Position : IBoardPosition
		{
			public int Row { get; private set; }
			public int Column { get; private set; }

			public Position(int row, int column)
			{
				Row = row;
				Column = column;
			}
			public Position(char row, int column)
			{
				//TODO: Implement this later
			}
		}
	}

	class Attack : IBoardPosition
	{
		public int Row { get; private set; }
		public int Column { get; private set; }
		public AttackResult Result { get; set; }

		public Attack(int row, int column)
		{
			Row = row;
			Column = column;
			Result = AttackResult.Unknown;
		}
	}
}
