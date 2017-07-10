using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.Constants;

namespace Battleship.Contracts
{
	interface IBattleShip
	{
		UnitType Type { get; }
		UnitHealth Health { get; }
		List<IBattleShipPart> Parts { get; }
		int Height { get; }
		int Width { get; }
	}

	interface IBattleShipPart
	{
		UnitType Type { get; }
		UnitHealth Health { get; }
		bool AbsorbHit();
	}

	interface IBattleBoard
	{
		IBattleShipPart this[int i, int j] { get; }
		bool AddShip(IBattleShip ship, IBoardPosition position);
	}

	interface IBoardPosition
	{
		int Row { get; }
		int Column { get; }
	}
}
