using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Battleship
{
    class Game
    {
        private readonly PlayerBattleArea player1;
        private readonly PlayerBattleArea player2;

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
                    previousWinner = defender;
                    Console.WriteLine("{0} has no more missiles left to launch", attacker.Id);
                }
                if (defender.ShipsAlive < 1)
                {
                    Console.WriteLine("{0} won the battle", attacker.Id);
                    break;
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
}