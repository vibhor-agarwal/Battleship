using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Battleship
{
	enum AttackResult
	{
		Unknown,
		Miss,
		Hit
	}

	enum ShipHealth
	{
		Fresh,
		Hit,
		Destroyed
	}

	enum BattleShipType
	{
		P = 1,
		Q = 2
	}
}