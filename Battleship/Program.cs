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
            var player1 = new PlayerBattleArea(5, 5);

            player1.AddShip(new BattleShip(BattleShipType.Q, 1, 1), new BattleBoard.BoardPosition(1, 1));
            player1.AddShip(new BattleShip(BattleShipType.P, 2, 1), new BattleBoard.BoardPosition(4, 4));

            var player2 = new PlayerBattleArea(5, 5);

            player2.AddShip(new BattleShip(BattleShipType.Q, 1, 1), new BattleBoard.BoardPosition(2, 2));
            player2.AddShip(new BattleShip(BattleShipType.P, 2, 1), new BattleBoard.BoardPosition(3, 3));

            var game = new Game(new PlayerBattleArea(5, 5), new PlayerBattleArea(5, 5));
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

        #region Board
        public void AddShip(BattleShip ship, BattleBoard.BoardPosition position)
        {
            Board.AddShip(ship, position);
        }
        #endregion

        #region Ship

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
            for (int i = position.Row - 1; i < position.Row + ship.Height; i++)
            {
                for (int j = position.Column - 1; j < position.Column + ship.Width; j++)
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
        public class BoardPosition : IBoardPosition
        {
            public int Row { get; private set; }
            public int Column { get; private set; }

            public BoardPosition(int row, int column)
            {
                Row = row;
                Column = column;
            }
            public BoardPosition(char row, int column)
            {
                //TODO: Implement this later
            }
        }
    }

    class Game
    {
        public Game(PlayerBattleArea playerA, PlayerBattleArea playerB)
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
}
