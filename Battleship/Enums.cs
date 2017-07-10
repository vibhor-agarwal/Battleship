using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Battleship.Constants
{
	enum AttackResult
	{
		Unknown,
		Miss,
		Hit
	}

	enum UnitHealth
	{
		Fresh,
		Hit,
		Destroyed
	}

	enum UnitType
	{
		P = 1,
		Q = 2
	}
}