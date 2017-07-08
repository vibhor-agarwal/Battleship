using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new PlayerBattleArea("Player1", 5, 5);

            player1.AddShip(new BattleShip(BattleShipType.Q, 1, 1), new BattleBoard.Position(1, 1));
            player1.AddShip(new BattleShip(BattleShipType.P, 2, 1), new BattleBoard.Position(4, 4));

            player1.AddAttackSequence(new List<Attack> {
                new Attack(1,2),
                new Attack(2,2),
                new Attack(2,2),
                new Attack(2,3)
            });

            var player2 = new PlayerBattleArea("Player2", 5, 5);

            player2.AddShip(new BattleShip(BattleShipType.Q, 1, 1), new BattleBoard.Position(2, 2));
            player2.AddShip(new BattleShip(BattleShipType.P, 2, 1), new BattleBoard.Position(3, 3));

            player2.AddAttackSequence(new List<Attack> {
                new Attack(1,1),
                new Attack(2,2),
                new Attack(2,3),
                new Attack(1,1),
                new Attack(4,1),
                new Attack(5,1),
                new Attack(4,4),
                new Attack(4,4),
                new Attack(4,5),
                new Attack(4,5)
            });

            var game = new Game(player1, player2);
            game.AutoPlay();
            Console.ReadLine();
        }
    }
}
