using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    interface IBattleShip
    {
        BattleShipType Type { get; }
        ShipHealth Health { get; }
        List<IShipPart> Parts { get; }
        int Height { get; }
        int Width { get; }
        int Size { get; }
        bool AbsorbHit();
    }

    interface IShipPart
    {
        BattleShipType Type { get; }        
        ShipHealth Health { get; }
        bool AbsorbHit();
    }

    interface IBattleBoard
    {
        IBattleShip this[int i, int j] { get; }
        bool AddShip(IBattleShip ship, IBoardPosition position);
    }

    interface IBoardPosition
    {
        int Row { get; }
        int Column { get; }
    }
}
