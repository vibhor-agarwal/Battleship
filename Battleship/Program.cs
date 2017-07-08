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
            var player1 = new PlayerBattleArea(5, 5);

            player1.AddShip(new BattleShip(BattleShipType.Q, 1, 1), new BattleBoard.Position(1, 1));
            player1.AddShip(new BattleShip(BattleShipType.P, 2, 1), new BattleBoard.Position(4, 4));

            player1.AddAttackSequence(new List<Attack> {
                new Attack(1,2),
                new Attack(2,2),
                new Attack(2,2),
                new Attack(2,3)
            });

            var player2 = new PlayerBattleArea(5, 5);

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
        }
    }

    class PlayerBattleArea
    {
        public BattleBoard Board { get; private set; }

        public PlayerBattleArea(int height, int width)
        {
            Board = new BattleBoard(height, width);
        }

        #region Configuration
        public void AddShip(BattleShip ship, BattleBoard.Position position)
        {
            Board.AddShip(ship, position);
        }

        public void AddAttackSequence(List<Attack> attackSequence)
        {

        }
        #endregion

        #region Gameplay
        public void HandleAttack()
        {

        }
        #endregion
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

        public BattleShip(BattleShipType type, int width, int height)
        {
            Type = type;
            Height = height;
            Width = width;
        }
    }

    partial class BattleBoard : IBattleBoard
    {
        private IBattleShip[,] board { get; set; }

        public BattleBoard(int height, int width)
        {
            board = new BattleShip[height, width];
        }

        public IBattleShip this[int row, int col]
        {
            get
            {
                return board[row, col];
            }
            set
            {
                board[row, col] = value;
            }
        }

        public bool AddShip(IBattleShip ship, IBoardPosition position)
        {
            var y = position.Row - 1;
            var x = position.Column - 1;

            for (int i = y; i < y + ship.Height; i++)
            {
                for (int j = x; j < x + ship.Width; j++)
                {
                    this[i, j] = ship;
                }
            }

            //this[position.Row - 1, position.Column - 1] = ship;
            return true;
        }
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

    class Game
    {
        public Game(PlayerBattleArea player1, PlayerBattleArea player2)
        {

        }

        public void AutoPlay()
        {
        }
    }

    interface IBoardPosition
    {
        int Row { get; }
        int Column { get; }
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
        bool AddShip(IBattleShip ship, IBoardPosition position);
    }

    class AttackSequence : List<Attack>
    {

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
    enum AttackResult
    {
        Miss,
        Hit,
        Unknown
    }
}
