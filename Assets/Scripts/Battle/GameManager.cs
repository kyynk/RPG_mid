using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleState
{
    public class GameManager : MonoBehaviour
    {
        private EventHandler eventHandler;
        private Player[] players;
        private int playerTurn; // 0 or 1 (player 1 or player 2)
        private int turnCount;
        private int matchCount;
        private int firstPlayer;

        void Start()
        {
            eventHandler = new EventHandler();
            players = new Player[2];
            players[0] = new Player();
            players[1] = new Player();
            playerTurn = 0;
            turnCount = 0;
            matchCount = 0;
            firstPlayer = 0;
        }

        public void SelectWhoFirst(int player)
        {
            firstPlayer = player;
            playerTurn = player;
        }

        public void NewTurn()
        {
            // every turn need two players to attack each other, so we need to get the current player and the opponent
            Player currentPlayer = players[playerTurn];
            Player opponent = players[(playerTurn + 1) % 2];
            
            eventHandler.OnPlayerAttack(currentPlayer, opponent);

            Debug.Log("Player " + playerTurn + " attack!");
            Debug.Log("Player 0" + " HP: " + players[0].hp);
            Debug.Log("Player 1" + " HP: " + players[1].hp);

            if (firstPlayer != playerTurn)
            {
                turnCount++;
            }
            NewMatch();

            playerTurn = (playerTurn + 1) % 2;
        }

        public void NewMatch()
        {
            // if 10 turn or one player hp < 0, then the match is over
            if (turnCount == 10 || players[0].hp <= 0 || players[1].hp <= 0)
            {
                WhoWin();
                turnCount = 0;
                matchCount++;
                foreach (Player player in players)
                {
                    player.ResetStatus();
                }
            }
            //else
            //{
            //    turnCount++;
            //    NewTurn();
            //}
        }

        public void WhoWin()
        {
            // has results: p1 win, p2 win, draw
            if (players[1].hp <= 0)
            {
                Debug.Log("Player 0 win!");
            }
            else if (players[0].hp <= 0)
            {
                Debug.Log("Player 1 win!");
            }
            else
            {
                Debug.Log("Draw!");
            }
        }
    }
}
