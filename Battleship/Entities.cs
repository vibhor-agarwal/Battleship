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
		#region Props
		public string Id { get; private set; }
		public Queue<Attack> AttackSequence { get; private set; }
		public int ShipsAlive { get { return Ships.Count(x => x.Health != UnitHealth.Destroyed); } }
		#endregion

		#region Private
		private BattleBoard Board { get; set; }
		private List<IBattleShip> Ships { get; set; }
		#endregion

		public PlayerBattleArea(string id, int height, int width)
		{
			Id = id;
			Board = new BattleBoard(height, width);
			Ships = new List<IBattleShip>();
		}

		#region Configuration
		public void AddShip(BattleShip ship, BattleBoard.Position position)
		{
			if (Board.AddShip(ship, position))
			{
				Ships.Add(ship);
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
		#region Props
		public UnitType Type { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }
		public List<IBattleShipPart> Parts { get; private set; }
		#endregion

		public UnitHealth Health
		{
			get
			{
				return Parts.All(x => x.Health == UnitHealth.Destroyed) ?
						UnitHealth.Destroyed : Parts.Any(x => x.Health == UnitHealth.Destroyed) ?
						UnitHealth.Hit : UnitHealth.Fresh;
			}
		}

		public BattleShip(UnitType type, int width, int height)
		{
			Type = type;
			Height = height;
			Width = width;
			BuildParts();
		}

		private void BuildParts()
		{
			Parts = new List<IBattleShipPart>();
			for (int i = 0; i < Width * Height; i++)
			{
				Parts.Add(new BattleShipPart(this));
			}
		}
	}

	class BattleShipPart : IBattleShipPart
	{
		#region Props
		public UnitHealth Health { get; private set; }
		public UnitType Type { get { return Parent.Type; } }
		#endregion

		private IBattleShip Parent { get; set; }
		private int RemainingHits { get; set; }

		public BattleShipPart(IBattleShip parent)
		{
			Parent = parent;
			RemainingHits = parent.Type == UnitType.P ? 1 : 2;
			Health = UnitHealth.Fresh;
		}

		public bool AbsorbHit()
		{
			RemainingHits--;
			Health = RemainingHits > 0 ? UnitHealth.Hit : UnitHealth.Destroyed;
			return RemainingHits > 0 ? true : false;
		}
	}

	partial class BattleBoard : IBattleBoard
	{
		private IBattleShipPart[,] Board { get; set; }

		public BattleBoard(int height, int width)
		{
			Board = new IBattleShipPart[height, width];
		}

		public IBattleShipPart this[int row, int col]
		{
			get { return Board[row, col]; }
			private set { Board[row, col] = value; }
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
			var attackHandled = true;
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
