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
        }
    }

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
        public int Size { get; private set; }
        public int RemainingHits { get; private set; }
        public ShipHealth Health { get; private set; }

        public BattleShip(BattleShipType type, int width, int height)
        {
            Type = type;
            Height = height;
            Width = width;
            Size = Height * Width;
            RemainingHits = Type == BattleShipType.P ? 1 : 2;
            Health = ShipHealth.Fresh;
        }

        public bool AbsorbHit()
        {
            RemainingHits--;
            return RemainingHits > 0 ? true : false;
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

        #region Gameplay
        public bool HandleAttack(Attack attack)
        {
            bool attackHandled = true;
            var ship = this[attack.Row - 1, attack.Column - 1];
            if (ship != null)
            {
                attack.Result = AttackResult.Hit;
                if (!ship.AbsorbHit())
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

    class Game
    {
        private PlayerBattleArea player1;
        private PlayerBattleArea player2;

        public Game(PlayerBattleArea player1, PlayerBattleArea player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        public void AutoPlay()
        {
            PlayerBattleArea attacker;
            PlayerBattleArea defender;
            PlayerBattleArea previousWinner = null;

            while (player1.AttackSequence.Any() || player2.AttackSequence.Any())
            {
                SelectPlayers(out attacker, out defender, previousWinner);
                if (attacker.AttackSequence.Count > 0)
                {
                    var attack = attacker.AttackSequence.Dequeue();
                    defender.HandleAttack(attack);
                    previousWinner = attack.Result == AttackResult.Hit ? attacker : defender;

                    Console.WriteLine("{0} fires a missle with target {1},{2} which got {3}", attacker.Id, attack.Row, attack.Column, attack.Result.ToString().ToLower());
                }
                else
                {
                    Console.WriteLine("{0} has no more missiles left to launch", attacker.Id);
                    previousWinner = defender;
                }
                if (defender.ShipsAlive < 1)
                {
                    Console.WriteLine("{0} won the battle", attacker.Id);
                }
            }
        }

        private void SelectPlayers(out PlayerBattleArea attacker, out PlayerBattleArea defender, PlayerBattleArea previousWinner = null)
        {
            if (previousWinner == null)
            {
                attacker = player1;
                defender = player2;
            }
            else
            {
                attacker = previousWinner;
                defender = attacker == player1 ? player2 : player1;
            }
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
        ShipHealth Health { get; }
        int Height { get; }
        int Width { get; }
        int Size { get; }
        bool AbsorbHit();
    }

    interface IBattleBoard
    {
        IBattleShip this[int i, int j] { get; }
        bool AddShip(IBattleShip ship, IBoardPosition position);
    }

    //class AttackSequence : List<Attack>
    //{

    //}

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

    class TextConsole
    {

    }
}
