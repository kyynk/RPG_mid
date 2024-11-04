using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleState
{
    public class GameManager : MonoBehaviour
    {
        private EventHandler eventHandler;
        private IPlayer[] players;
        private int playerTurn; // 0 or 1 (player 1 or player 2)
        private int turnCount;
        private int matchCount;
        private int firstPlayer;

        void Start()
        {
            eventHandler = new EventHandler();
            players = new IPlayer[2];
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

        public void NextTurn(string action)
        {
            // every turn need two players to attack each other, so we need to get the current player and the opponent
            IPlayer currentPlayer = players[playerTurn];
            IPlayer opponent = players[(playerTurn + 1) % 2];
            
            if (action == "attack")
            {
                Debug.Log("Player " + playerTurn + " attack!");
                eventHandler.OnPlayerAttack(currentPlayer, opponent);
            }
            else if (action == "defence")
            {
                Debug.Log("Player " + playerTurn + " defence!");
                currentPlayer.IsDefend = true;
            }
            else
            {
                Debug.Log("Invalid action!");
            }

            Debug.Log("Player 0" + " HP: " + players[0].HP);
            Debug.Log("Player 1" + " HP: " + players[1].HP);

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
            if (turnCount == 10 || players[0].HP <= 0 || players[1].HP <= 0)
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
            if (players[1].HP <= 0)
            {
                Debug.Log("Player 0 win!");
            }
            else if (players[0].HP <= 0)
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
